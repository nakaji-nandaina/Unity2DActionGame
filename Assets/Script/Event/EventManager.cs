using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //event�p�̃t���O�i�J�n�����t���O�ƃC�x���g�����t���O�j
    public List<bool> StartEventFlag { get; private set;}
    public List<bool> FinishedEventFlag { get; private set; }
    /*
     * 0:���Q�[���J�n�C�x���g�t���O
     * 1:�����l��b�t���O
     * 2:���_���W���������t���O
     * 3:���{�X�����t���O
     * 4:���퐧��t���O
     * 5:���ʃN�G�X�g�t���O
     * 6:����������t���O
     */

    //event1�p
    [SerializeField]
    private GameObject firstNPC;
    [HideInInspector]
    public GameObject FirstNPC; 


    void Awake()
    {
        //���̃C�x���g�t���O�쐬
        InitStartEventFlag(new List<bool>(new bool[10]));
        InitFinishedEventFlag(new List<bool>(new bool[10]));
    }

    void Update()
    {
        
    }

    public void InitStartEventFlag(List<bool> startEventFlag)
    {
        StartEventFlag = startEventFlag;
    }
    public void InitFinishedEventFlag(List<bool> finishedEventFlag)
    {
        FinishedEventFlag = finishedEventFlag;
    }

    public void setStartEventFlag(int idx)
    {
        StartEventFlag[idx] = true;
    }

    public void setFinishedEventFlag(int idx)
    {
        FinishedEventFlag[idx] = true;
    }

    public IEnumerator PlayerWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameManager.instance.Player.changePS(PlayerController.PS.stop);
    }
    public IEnumerator PlayerNorm(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameManager.instance.Player.changePS(PlayerController.PS.normal);
    }

    //�C�x���g#1 �͂��߂ăQ�[�����͂��߂��ꍇ�ɉq�����v���C���[�̑O�Ɍ���C��b���n�܂�
    public void FirstGameEvent()
    {
        setFinishedEventFlag(0);
        StartCoroutine(PlayerWait(0.1f));
        StartCoroutine(FirstEventStart());
    }

    public void GoalEihei()
    {
        StartCoroutine(FirstConv());
    }
    public void EndEihei()
    {
        StartCoroutine(PlayerWait(0.1f));
        StartCoroutine(PlayerNorm(2f));
        List<int> x = new List<int>();
        List<int> y = new List<int>();
        x.Add(-19); x.Add(-19);y.Add(-22);y.Add(-6);
        FirstNPC.GetComponent<EventNPC>().InitNPC(x,y,"DeleteNPC");
    }
    
    public IEnumerator FirstEventStart()
    {
        yield return new WaitForSeconds(0.1f);
        FirstNPC=Instantiate(firstNPC);
        FirstNPC.transform.position =new Vector2(-19,-6);
        Debug.LogError(FirstNPC);
    }

    public IEnumerator FirstConv()
    {
        yield return new WaitForSeconds(1f);
        string[] lines=new string[2] { "����ɂ��́I���낻�뉤�l�Ƃ̉y���̎��ԂɂȂ�܂�!\n��������������", "����ł́A��낵�����˂������܂�" };
        string CharName = "���낤";
        bool Choice = false;
        string[] YesLines = new string[1];
        string[] NoLines = new string[1];
        string funcName = "FinishStartConv";
        string YesfuncName = "";
        string NofuncName = "";
        GameManager.instance.ShowDialog(lines, CharName, Choice, YesLines, NoLines, this.gameObject, funcName, YesfuncName, NofuncName);
    }


}

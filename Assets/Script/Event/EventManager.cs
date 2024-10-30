using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    /*
     * 0:���Q�[���J�n�C�x���g�t���O
     * 1:�����l��b�C�x���g�t���O
     * 2:���_���W���������t���O
     * 3:���{�X�����t���O
     * 4:���퐧��t���O
     * 5:���ʃN�G�X�g�t���O
     * 6:����������t���O
     */

    //event0�p
    [SerializeField]
    private GameObject firstNPC;
    [HideInInspector]
    public GameObject FirstNPC;
    //event1�p
    [HideInInspector]
    public GameObject KingNPC;
    [HideInInspector]
    public GameObject FirstItem;



    public void setStartEventFlag(int idx)
    {
        GameManager.startEventFlag[idx] = true;
    }

    public void setFinishedEventFlag(int idx)
    {
        GameManager.finishedEventFlag[idx] = true;
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
    public IEnumerator OpenTutorial(float waitTime,int ind)
    {
        yield return new WaitForSeconds(waitTime);
        GameManager.instance.tutorialManager.OpenTutorial(ind,5f);
    }

    //�C�x���g#0 �͂��߂ăQ�[�����͂��߂��ꍇ�ɉq�����v���C���[�̑O�Ɍ���C��b���n�܂�
    public void FirstGameEvent()
    {
        setFinishedEventFlag(0);
        setStartEventFlag(1);
        StartCoroutine(PlayerWait(0.1f));
        StartCoroutine(FirstEventStart());
    }

    public void GoalEihei()
    {
        Debug.LogError("EiheiGoal");
        StartCoroutine(FirstConv());
    }
    public void EndEihei()
    {
        StartCoroutine(PlayerWait(0.1f));
        StartCoroutine(PlayerNorm(2f));
        StartCoroutine(OpenTutorial(2f, 0));
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
        string[] lines=new string[2] { "����ɂ��́I \n���������A�������܂Ƃ̂�������̂�����ɂȂ�܂�!   \n�������т�������", "����ł́A��낵�����˂������܂�" };
        string CharName = "���낤";
        bool Choice = false;
        string[] YesLines = new string[1];
        string[] NoLines = new string[1];
        string funcName = "FinishStartConv";
        string YesfuncName = "";
        string NofuncName = "";
        GameManager.instance.ShowDialog(lines, CharName, Choice, YesLines, NoLines, this.gameObject, funcName, YesfuncName, NofuncName);
    }

    //�C�x���g#1 ���l�Ƃ̏��߂Ẳ�b
    public void KingFirstFunc(GameObject kingObj)
    {
        setFinishedEventFlag(1);
        KingNPC = kingObj;
        string[] _lines = { "�����I �������A���傤�˂��B  \n���߂łƂ��B �悤�₭ 15�����ɂȂ����̂��ȁB",
            "����ŁA���ʂ��� �����ɂ�܂��ƂȂ�A  \n�߂����イ�� ���ǂނ����� �����̂��B  " ,
            "���̂��ɂɂ́A���񂱂��̂��납�� �Â��߂����イ�� ����B   \n�߂����イ �ɂ� �������� �Ԃ��₴���ق��� �˂ނ��Ă���B",
            "���� ��������Ԃɂ́A���イ���傭�� �����ق� �� \n���񂴂����Ă��� �Ƃ����Ă���B",
            "�����������A��������Ԃ܂� ���ǂ���� ���̂� �ЂƂ�� ����ʁB  \n�߂����イ�ɂ́A ������Ȃ܂��̂� �����������Ă���A\n���傤���񂵂Ⴝ���� ���񂱂��� �͂΂�ł���̂���B",
            "�����ŁA���ʂ��̂��ł� �݂���� ���݂̂�����I�I   \n��������Ԃ� �߂����A \n���� �����ق��� �Ăɂ���� �ق����̂��B", 
            "�������A�߂����イ�� �Ȃ��� �����񂶂�B   \n��������� �Ԃ��� ����A���Ԃ�� �������Ȃ���΁A \n�������܂� ���ǂ���� ����낤�B",
            "�ǂ����H����Ă���邩�H"
        };
        string[] _no_lines = { "�ӂ�... �܂� �����񂪂Ȃ����B \n�����A���ʂ��قǂ� �����̂������� ���̂� �������� ����ʁB \n�ق��̂��̂ɂ� �߂����イ�� �Ƃ��͂ł��ʂ���낤�B", "���������� �悭���񂪂��Ă݂Ă� �ǂ�����낤���H \n������� ���������A���ꂾ���̂��������邶��낤�B \n���ʂ��� �䂤���� �ǂ��� �킵�� �݂��Ă� ����ʂ��H" };
        string[] _yes_lines = { "�����A�悭�� �����Ă��ꂽ�I \n�������� �킵�� �݂��� ���傤�˂񂶂�B", "�����炭�A���̂����ɂ� ���܂��܂� ������� ����� �܂��Ă���B \n�������A���ʂ��� ������̂肱�����邿����� �����Ă���B \n�ǂ����A������� ������ ������ł���B","�݂炢�� �䂤����� ���ǂł� ���̂����т���낤�B\n�킵�� �킩����������Ă��� �Ԃ��Ƃ����Ăނ���B" };
        KingNPC.GetComponent<DialogActivater>().InitActivater(_lines, true,_yes_lines, _no_lines, "FirstKingConv", "NullReturn", "RoopConv");
    }
    public void SetFirstItem(GameObject firstitem)
    {
        FirstItem = firstitem;
    }

    public void KingAfterFirstFunc()
    {
        FirstItem.SetActive(true);

        StartCoroutine(OpenTutorial(0.5f, 2));
        string[] _lines = { "���傤�˂��B\n���������Ă��邼�B" };
        KingNPC.GetComponent<DialogActivater>().InitActivater(_lines, false, new string[1], new string[1], "NullReturn", "NullReturn", "NullReturn");
    }


}

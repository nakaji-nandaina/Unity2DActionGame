using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    //event用のフラグ（開始条件フラグとイベント完了フラグ）
    public List<bool> StartEventFlag { get; private set;}
    public List<bool> FinishedEventFlag { get; private set; }
    /*
     * 0:初ゲーム開始イベントフラグ
     * 1:初王様会話フラグ
     * 2:初ダンジョン潜入フラグ
     * 3:第一ボス討伐フラグ
     * 4:武器制作フラグ
     * 5:特別クエストフラグ
     * 6:下水道解放フラグ
     */

    //event1用
    [SerializeField]
    private GameObject firstNPC;
    [HideInInspector]
    public GameObject FirstNPC; 


    void Awake()
    {
        //仮のイベントフラグ作成
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

    //イベント#1 はじめてゲームをはじめた場合に衛兵がプレイヤーの前に現れ，会話が始まる
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
        string[] lines=new string[2] { "こんにちは！そろそろ王様との謁見の時間になります!\nご準備ください", "それでは、よろしくおねがいします" };
        string CharName = "たろう";
        bool Choice = false;
        string[] YesLines = new string[1];
        string[] NoLines = new string[1];
        string funcName = "FinishStartConv";
        string YesfuncName = "";
        string NofuncName = "";
        GameManager.instance.ShowDialog(lines, CharName, Choice, YesLines, NoLines, this.gameObject, funcName, YesfuncName, NofuncName);
    }


}

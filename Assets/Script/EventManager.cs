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
    public GameObject firstNPC;



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

    public void FirstGameEvent()
    {
        setFinishedEventFlag(0);
        StartCoroutine(FirstConv());
    }

    //FirstGameEventから呼ばれてはじめてゲームをはじめた場合に衛兵がプレイヤーの前に現れ，会話が始まる
    public IEnumerator FirstConv()
    {
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.Player.changePS(PlayerController.PS.stop);
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

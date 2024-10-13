using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{

    /*
     * 0:初ゲーム開始イベントフラグ
     * 1:初王様会話イベントフラグ
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
    [HideInInspector]
    public GameObject KingNPC;


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

    //イベント#0 はじめてゲームをはじめた場合に衛兵がプレイヤーの前に現れ，会話が始まる
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
        string[] lines=new string[2] { "こんにちは！ \nもうすぐ、おうさまとのえっけんのじかんになります!   \nごじゅんびください", "それでは、よろしくおねがいします" };
        string CharName = "たろう";
        bool Choice = false;
        string[] YesLines = new string[1];
        string[] NoLines = new string[1];
        string funcName = "FinishStartConv";
        string YesfuncName = "";
        string NofuncName = "";
        GameManager.instance.ShowDialog(lines, CharName, Choice, YesLines, NoLines, this.gameObject, funcName, YesfuncName, NofuncName);
    }

    //イベント#1 王様との初めての会話
    public void KingFirstFunc(GameObject kingObj)
    {
        setFinishedEventFlag(1);
        KingNPC = kingObj;
        string[] _lines = { "おお！ きたか、しょうねんよ。  \nおめでとう。 ようやく 15さいになったのだな。",
            "これで、おぬしも いちにんまえとなり、  \nめいきゅうに いどむけんりを えたのだ。  " ,
            "このくにには、けんこくのころから つづくめいきゅうが ある。   \nめいきゅう には おおくの ぶきやざいほうが ねむっておる。",
            "その さいしんぶには、きゅうきょくの ざいほう が \nそんざいしている といわれておる。",
            "だがしかし、さいしんぶまで たどりつけた ものは ひとりも おらぬ。  \nめいきゅうには、 きけんなまものが せいそくしており、\nちょうせんしゃたちの しんこうを はばんでおるのじゃ。",
            "そこで、おぬしのうでを みこんで たのみがある！！   \nさいしんぶを めざし、 \nその ざいほうを てにいれて ほしいのだ。", 
            "ただし、めいきゅうの なかは きけんじゃ。   \nしっかりと ぶきを つくり、じぶんを きたえなければ、 \nさいごまで たどりつけぬ じゃろう。",
            "どうだ？やってくれるか？"
        };
        KingNPC.GetComponent<DialogActivater>().InitActivater(_lines, false, new string[1], new string[1], "FirstKingConv", "NullReturn", "NullReturn");
    }

    public void KingAfterFirstFunc()
    {
        string[] _lines = { "しょうねんよ。\nきたいしておるぞ。" };
        KingNPC.GetComponent<DialogActivater>().InitActivater(_lines, false, new string[1], new string[1], "NullReturn", "NullReturn", "NullReturn");
    }


}

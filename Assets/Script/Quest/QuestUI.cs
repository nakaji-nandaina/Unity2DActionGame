using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public GameObject QuestPanel;
    public Text explaneText;
    public Text titleText;
    public Text ClientText;
    public GameObject TargetCont;
    public GameObject target;
    public GameObject[] papers;
    
    public void setQuestUI(OrderQuest orderQuest)
    {
        titleText.text = "クエスト名";
        explaneText.text = "";
        ClientText.text = "いらいしゃ";
        for (int i = 0; i< 3; i++)
        {
            if(i>= orderQuest.QuestList.Count)
            {
                papers[i].SetActive(false);
                continue;
            }
            int ind = i;
            papers[i].SetActive(true);
            if(orderQuest.CompleteQuest(orderQuest.QuestList[i]))papers[i].transform.GetChild(0).gameObject.SetActive(true);
            else papers[i].transform.GetChild(0).gameObject.SetActive(false);
            papers[i].GetComponent<Button>().onClick.AddListener(()=>ClickPaper(ind));
        }
    }
    public void closeQuestUI()
    {
        for (int i = 0; i < 3; i++)
        {
            papers[i].SetActive(false);
            papers[i].GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
    public void ClickPaper(int ind)
    {
        for(int i = TargetCont.transform.childCount; i > 0; i--)
        {
            Destroy(TargetCont.transform.GetChild(i-1).gameObject);
        }
        OrderQuest order = GameManager.instance.Player.orderQuest;
        Quest quest=order.QuestList[ind];
        
        switch (quest.GetType().ToString()) {
            case nameof(HuntQuest):
                HuntQuest hq = (HuntQuest)quest;
                for (int i = 0; i < hq.huntEnemys.Count; i++)
                {
                    GameObject newButtonObject = Instantiate(target);
                    newButtonObject.transform.SetParent(TargetCont.transform,false);
                    newButtonObject.transform.GetChild(0).GetComponent<Text>().text = hq.huntEnemys[i].huntednum.ToString() + "/" + hq.huntEnemys[i].num.ToString();
                    newButtonObject.transform.GetChild(1).GetComponent<Text>().text = hq.huntEnemys[i].enemy.enemyName;
                }
            break;
        }

        
        titleText.text = quest.questName;
        explaneText.text = quest.information;
        ClientText.text = quest.client;
        //Debug.LogError(ind);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class QuestBoard : MonoBehaviour
{
    public GameObject QuestBoardPanel;
    public GameObject QuestOrderPanel;
    public List<GameObject> QuestPapers;
    public List<Sprite> PaperImages;
    [HideInInspector]
    public List<Quest> BoardQuests;
    public GameObject QuestName,ClientName,QuestCont,RewardCont,QuestInfo,AcceptStamp,CloseButton,AcceptButton;
    public AudioClip StampSE;

    private Vector2 iniPos;
    private float ShakeCounter=0.2f;

    private float ShakeTime = 0.2f;
    private void Update()
    {
        if(ShakeCounter<ShakeTime)ShakePaper();
        else
        {
            QuestOrderPanel.transform.localPosition=iniPos;
        }
    }
    public void ShakePaper()
    {
        float speed = 10f;
        float amplitude = 15f;
        float offsetx = Random.Range(0f, 256f);
        float offsety = Random.Range(0f, 256f);
        ShakeCounter += Time.deltaTime;
        float nx = 2*amplitude * (Mathf.PerlinNoise(Time.time*speed+offsetx, 0) - 0.5f);
        float ny = 2*amplitude * (Mathf.PerlinNoise(Time.time*speed+offsety, 0) - 0.5f);
        QuestOrderPanel.transform.localPosition =new Vector2(nx,ny);
        
    }
    public void SetQuestBoard()
    {
        BoardQuests = GameManager.instance.Player.BoardQuests;
        OrderQuest ordered = GameManager.instance.Player.orderQuest;
        iniPos = QuestOrderPanel.transform.localPosition;
        CloseButton.GetComponent<Button>().onClick.RemoveAllListeners();
        CloseButton.GetComponent<Button>().onClick.AddListener(() => { QuestOrderPanel.SetActive(false); });

        for (int i = 0; i < QuestPapers.Count; i++)
        {
            if (i < BoardQuests.Count)
            {
                QuestPapers[i].SetActive(true);
                int rand = Random.Range(0,PaperImages.Count);
                QuestPapers[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = PaperImages[rand];
                QuestPapers[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text=BoardQuests[i].questName;
                if(ordered.IsOrdered(BoardQuests[i]))QuestPapers[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                else QuestPapers[i].transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                int id = i;
                QuestPapers[i].transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                QuestPapers[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(()=> { OpenPaper(id); });
            }
            else
            {
                QuestPapers[i].SetActive(false);
            }
        }

    }
    public void OpenPaper(int id)
    {
        OrderQuest ordered = GameManager.instance.Player.orderQuest;
        QuestOrderPanel.SetActive(true);
        if (ordered.IsOrdered(BoardQuests[id]))
        {
            AcceptStamp.SetActive(true);
            AcceptButton.SetActive(false);
        }
        else
        {
            AcceptStamp.SetActive(false);
            AcceptButton.SetActive(true);
            AcceptButton.GetComponent<Button>().onClick.RemoveAllListeners();
            AcceptButton.GetComponent<Button>().onClick.AddListener(() => { Accept(id); });
        }
        QuestName.GetComponent<Text>().text = BoardQuests[id].questName;
        ClientName.GetComponent<Text>().text = BoardQuests[id].client;
        RewardCont.GetComponent<Text>().text = BoardQuests[id].money.ToString()+"G"; 
        for (int i=0;i< BoardQuests[id].rewardItems.Count; i++)
        {
            RewardCont.GetComponent<Text>().text += "\n";
            RewardCont.GetComponent<Text>().text += BoardQuests[id].rewardItems[i].item.itemname+"x"+ BoardQuests[id].rewardItems[i].num.ToString();
        }
        QuestInfo.GetComponent<Text>().text = BoardQuests[id].information;
        switch (BoardQuests[id].GetType().ToString())
        {
            case nameof(HuntQuest):
                HuntQuest hunt = (HuntQuest)BoardQuests[id];
                QuestCont.GetComponent<Text>().text = "";
                for (int i = 0; i < hunt.huntEnemys.Count; i++)
                {
                    QuestCont.GetComponent<Text>().text += hunt.huntEnemys[i].enemy.enemyName + " x" + hunt.huntEnemys[i].num.ToString();
                    QuestCont.GetComponent<Text>().text += "\n";
                }
                break;
        }
            
        //QuestCont.GetComponent<>

    }
    public void Accept(int id)
    {
        OrderQuest ordered = GameManager.instance.Player.orderQuest;
        if (ordered.QuestList.Count >= ordered.max) return;
        if (ordered.IsOrdered(BoardQuests[id])) return;
        ordered.AddQuest(BoardQuests[id]);
        AcceptStamp.SetActive(true);
        GameManager.instance.PlayAudio(StampSE);
        //GameManager.instance.Impulse.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        ShakeCounter = 0f;
        QuestPapers[id].transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
        AcceptButton.SetActive(false);

    }
}

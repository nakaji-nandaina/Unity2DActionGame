using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "OrderQuest", menuName = "OrderQuest")]
public class OrderQuest : ScriptableObject
{
    public int max = 3;
    public List<Quest> QuestList = new List<Quest>();

    public void SetInitiate(List<int> ids,List<List<int>> content, DataBase dataBase)
    {
        QuestList = new List<Quest>();
        for (int i = 0; i < ids.Count; i++)
        {
            Quest _quest = dataBase.GetQuest(ids[i]);
            AddQuest(_quest);
            SetQuestContent(content[i], i);
        }
    }

    public bool AddQuest(Quest _quest)
    {
        if (QuestList.Count >= max) return false;
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i] != _quest) continue;
            Debug.LogError("受注済みクエストです。");
            return false;
        }
        switch (_quest.GetType().ToString())
        {
            case nameof(HuntQuest):
                HuntNumClear((HuntQuest)_quest);
                break;
        }
        QuestList.Add(_quest);
        return true;
    }
    public void SetQuestContent(List<int> content,int idx)
    {
        if (QuestList[idx] == null) return;
        Quest _quest = QuestList[idx];
        switch (_quest.GetType().ToString())
        {
            case nameof(HuntQuest):
                SetHuntNum((HuntQuest)_quest, content);
                break;
        }
    }

    public void CompleteQuests()
    {
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (!CompleteQuest(QuestList[i])) continue ;
            //報酬を受け取る
            RewardQuest(QuestList[i]);
            //再度クエストを受注可能にする
            switch (QuestList[i].GetType().ToString())
            {
                case nameof(HuntQuest):
                    HuntNumClear((HuntQuest)QuestList[i]);
                    break;
            }
            QuestList.RemoveAt(i);
        }
    }

    public void RewardQuest(Quest _quest)
    {
        for (int i = 0; i < _quest.rewardItems.Count; i++)
        {
            GameManager.instance.Player.inventory.AddItem(_quest.rewardItems[i].item, _quest.rewardItems[i].num);
        }
        GameManager.instance.UpdateMoneyUI(_quest.money + GameManager.currentMoney);
        Debug.LogError("ゲット");
    }

    public bool CompleteQuest(Quest _quest)
    {
        switch (_quest.GetType().ToString())
        {
            case nameof(HuntQuest):
                return CompleteHuntQuest((HuntQuest) _quest);
        }
        return true;
    }

    public bool CompleteHuntQuest(HuntQuest _quest)
    {
        for (int i = 0; i < _quest.huntEnemys.Count; i++)
        {
            if(_quest.huntEnemys[i].num > _quest.huntEnemys[i].huntednum)return false;
        }
        return true;
    }

    public void HuntEnemy(Enemy _enemy)
    {
        for(int i = 0; i < QuestList.Count; i++)
        {
            if (QuestList[i].GetType().ToString() != nameof(HuntQuest)) continue;
            HuntQuest _quest=(HuntQuest)QuestList[i];
            for (int j = 0; j < _quest.huntEnemys.Count; j++)
            {
                if (_enemy != _quest.huntEnemys[j].enemy) continue;
                _quest.huntEnemys[j].huntednum++;
            }
        }
    }

    public void HuntNumClear(HuntQuest _quest)
    {
        for(int i = 0; i < _quest.huntEnemys.Count; i++)
        {
            _quest.huntEnemys[i].huntednum = 0;
        }
    }

    public void SetHuntNum(HuntQuest _quest,List<int> content)
    {
        for (int i = 0; i < _quest.huntEnemys.Count; i++) _quest.huntEnemys[i].huntednum = content[i];
    }

    public List<int> GetHuntNum(HuntQuest _quest)
    {
        List<int> huntnum=new List<int>();
        for (int i = 0; i < _quest.huntEnemys.Count; i++) huntnum.Add(_quest.huntEnemys[i].huntednum);
        return huntnum;
    }

}

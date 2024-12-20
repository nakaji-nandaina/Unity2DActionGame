﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataBase", menuName = "CreateDataBase")]

public class DataBase : ScriptableObject
{
    public List<Item> itemDatabase = new List<Item>();
    public List<Recipe> recipeDatabase = new List<Recipe>();
    public List<Status> playerLvDatabase = new List<Status> ();
    public List<Skill> skillDatabase = new List<Skill>();
    public List<Enemy> enemyDatabase = new List<Enemy>();
    public List<WeaponData> weaponDatabase = new List<WeaponData>();
    public List<Quest> questDatabase = new List<Quest>();
    public List<SceneData> sceneDatabase = new List<SceneData>();
    
    //アイテム関係
    public List<int> GetItemIds(InventoryObject inventory)
    {
        List<int> itemIds= new List<int>();
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (GetItemId(inventory.Container[i].item) != -1)
            {
                itemIds.Add(GetItemId(inventory.Container[i].item));
            }
        }
        return itemIds;
    }

    public List<int> GetItemAmounts(InventoryObject inventory)
    {
        List<int> itemAmounts = new List<int>();
        for(int i = 0; i < inventory.Container.Count; i++)
        {
            itemAmounts.Add(inventory.Container[i].amount);
        }
        return itemAmounts;
    }

    public int GetItemId(Item item)
    {
        for(int i = 0; i < itemDatabase.Count; i++)
        {
            if (itemDatabase[i] == item)
            {
                return i;
            }
        }
        return -1;
    }


    public Item GetItemData(int id)
    {
        return itemDatabase[id];
    }

    //スキル関係
    public List<int> GetSkillIds(MySkills skills)
    {
        List<int> skillIds = new List<int>();
        for(int i=0; i < skills.skillContainer.Count; i++)
        {
            if (GetSkillId(skills.skillContainer[i].skill)!=-1)
            {
                skillIds.Add(GetSkillId(skills.skillContainer[i].skill));
            }
        }
        return skillIds;
    }

    public List<int> GetSkillLvs(MySkills skills)
    {
        List<int> skillLvs = new List<int>();
        for(int i=0; i< skills.skillContainer.Count; i++)
        {
            skillLvs.Add(skills.skillContainer[i].Lv);
        }
        return skillLvs;
    }

    public int GetSkillId(Skill skill)
    {
        for(int i=0; i < skillDatabase.Count; i++)
        {
            if (skillDatabase[i] == skill)
            {
                return i;
            }
        }
        return -1;
    }

    public Skill GetSkillData(int id)
    {
        return skillDatabase[id];
    }

    //敵関係
    public int GetHuntedEnemyNum(Enemy enemy)
    {
        int num = 0;
        for(int i = 0; i < enemyDatabase.Count; i++)
        {
            if (enemy != enemyDatabase[i]) continue;
            num = enemyDatabase[i].huntedNum;
            break;
        }
        return num;
    }

    public void AddHuntedEnemyNum(Enemy enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("enemyにデータ付与し忘れてるで");
            return;
        }
        for (int i = 0; i < enemyDatabase.Count; i++)
        {
            if (enemy != enemyDatabase[i]) continue;
            enemyDatabase[i].huntedNum++;
            //Debug.LogError(enemyDatabase[i].name+enemyDatabase[i].huntedNum.ToString());
            return;
        }
    }

    //武器関係
    public List<int> GetWeaponIds(WeaponPouch weaponPouch)
    {
        List<int> weaponIds=new List<int>();
        for (int i = 0; i < weaponPouch.Pouch.Count; i++)
        {
            weaponIds.Add(GetWeaponId(weaponPouch.Pouch[i]));
        }
        return weaponIds;
    }

    public int GetWeaponId(WeaponData weaponData)
    {
        return weaponDatabase.IndexOf(weaponData);
    }

    public WeaponData GetWeaponData(int id)
    {
        return weaponDatabase[id];
    }

    //Quest関係
    public int GetQuestId(Quest quest) 
    {
        for(int i = 0; i < questDatabase.Count; i++)
        {
            if (questDatabase[i] != quest) continue;
            return i;
        }
        return -1;
    }

    public List<int> GetQuestIds(OrderQuest orderQuest)
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < orderQuest.QuestList.Count; i++)
        {
            ids.Add(GetQuestId(orderQuest.QuestList[i]));
        }
        return ids;
    }
    public List<int> GetQuestIdsfromL(List<Quest> quests)
    {
        List<int> ids = new List<int>();
        for (int i = 0; i < quests.Count; i++)
        {
            ids.Add(GetQuestId(quests[i]));
        }
        return ids;
    }


    public List<Quest> GetBoardQuest(List<int> questIds)
    {
        List<int> BoardQuestIds = new List<int>();
        for(int i = 0; i < 8; i++)
        {
            int id= Random.Range(0, questDatabase.Count); 
            
            bool ok = true;
            for(int j = 0; j < questIds.Count; j++)
            {
                if (questIds[j] == id) ok = false;
                if (!ok) break;
            }
            for (int j = 0; j < BoardQuestIds.Count; j++)
            {
                if (BoardQuestIds[j] == id) ok = false;
                if (!ok) break;
            }
            if (ok) BoardQuestIds.Add(id); 
        }
        List<Quest> BoardQuests = new List<Quest>();
        for(int i = 0; i < BoardQuestIds.Count; i++)
        {
            BoardQuests.Add(GetQuest(BoardQuestIds[i]));
        }
        return BoardQuests;
    }

    public List<List<int>> GetQuestContent(OrderQuest orderQuest)
    {
        List<List<int>> questcont=new List<List<int>>();
        for (int i = 0; i < 3; i++)
        {
            if (i >= orderQuest.QuestList.Count)
            {
                questcont.Add(new List<int>());
                continue;
            }
            switch (orderQuest.QuestList[i].GetType().ToString())
            {
                case nameof(HuntQuest):
                    questcont.Add(orderQuest.GetHuntNum((HuntQuest)orderQuest.QuestList[i]));
                    break;
            }
        }
        return questcont;
    }

    public Quest GetQuest(int id)
    {
        return questDatabase[id];
    }
    

    //Scene関係
    public float GetSceneBright(string sceneName)
    {
        float res = -1;
        for(int i=0; i < sceneDatabase.Count; i++)
        {
            if (sceneDatabase[i].SceneName == sceneName)
            {
                res = sceneDatabase[i].SceneBrightness;
                break;
            }
        }
        return res;
    }
    public Vector2 GetSceneGoal(string sceneName)
    {
        Vector2 res = new Vector2(0,0);
        for (int i = 0; i < sceneDatabase.Count; i++)
        {
            if (sceneDatabase[i].SceneName == sceneName)
            {
                res = sceneDatabase[i].goalPoint;
                break;
            }
        }
        return res;
    }

    public AudioClip GetSceneBGM(string sceneName)
    {
        AudioClip res = null;
        for (int i = 0; i < sceneDatabase.Count; i++)
        {
            if (sceneDatabase[i].SceneName == sceneName)
            {
                res = sceneDatabase[i].BGM;
                break;
            }
        }
        return res;
    }
}

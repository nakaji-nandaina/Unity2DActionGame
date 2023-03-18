﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DataBase",menuName ="CreateDataBase")]
public class DataBase : ScriptableObject
{
    public List<Item> itemDatabase = new List<Item>();
    public List<Status> playerLvDatabase = new List<Status> ();
    public List<Skill> skillDatabase = new List<Skill>();

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

}

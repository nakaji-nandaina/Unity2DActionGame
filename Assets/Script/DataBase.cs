using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DataBase",menuName ="CreateDataBase")]
public class DataBase : ScriptableObject
{
    public List<Item> itemDatabase = new List<Item>();
    public List<Status> playerLvDatabase = new List<Status> ();
    public List<Skill> skillDatabase = new List<Skill>();
    public List<WeaponData> weaponDatabase = new List<WeaponData>();
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

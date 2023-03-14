using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="DataBase",menuName ="CreateDataBase")]
public class DataBase : ScriptableObject
{
    public List<Item> itemDatabase = new List<Item>();
    
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
}

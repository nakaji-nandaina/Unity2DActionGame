using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Inventory",menuName = "Inventory System/Inventory")]
public class InventoryObject: ScriptableObject
{
    [Header("持ち物")]
    public List<InventorySlot> Container = new List<InventorySlot>();
    public int AddItem(Item _item, int _amount)
    {
        bool hasItem = false;
        for(int i = 0; i < Container.Count; i++)
        {
            if(Container[i].item== _item)
            {
                Container[i].Addamount(_amount);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            Container.Add(new InventorySlot(_item, _amount));
        }
        return _amount;
    }

    public bool UsedItem(Item _item,int _amount)
    {
        bool used = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                if (Container[i].amount >= _amount)
                {
                    Container[i].Reduceamount(_amount);
                    used = true;
                    if (Container[i].amount == 0)
                    {
                        Container.RemoveAt(i);
                    }
                }
                break;
            }
        }
        return used;
    }
    public bool existItem(Item _item)
    {
        bool hasItem=false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                hasItem = true;
                break;
            }
        }
        return hasItem;
    }

    public void SetInitiate(List<int> ids, List<int> amounts,DataBase dataBase)
    {
        for(int i=0; i < ids.Count; i++)
        {
            Item item= dataBase.GetItemData(ids[i]);
            AddItem(item, amounts[i]);
        }
    }

}
[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int amount;
    public InventorySlot(Item _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }
    public void Addamount(int Value)
    {
        amount += Value;
    }
    public void Reduceamount(int Value)
    {
        amount -= Value;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="New Inventory",menuName = "Inventory System/Inventory")]
public class InventoryObject: ScriptableObject
{

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
}

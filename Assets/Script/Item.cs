using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName ="Item",menuName ="CreateItem")]
public class Item : ScriptableObject
{
    public enum Type
    {
        UserItem,
        CraftItem,
        KeyItem,
    }

    public String itemname;
    public Type type;
    public String information;
    public GameObject itemObj;
    public Item(Item item)
    {
        this.itemname = item.itemname;
        this.type = item.type;
        this.information = item.information;
        this.itemObj = item.itemObj;
    }
}

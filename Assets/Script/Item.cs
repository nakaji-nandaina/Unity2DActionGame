using System;
using UnityEngine;
using UnityEngine.UI;

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
    [Multiline(10)]
    public String information;
    public GameObject itemObj;
    public Sprite itemIcon;
    public Item(Item item)
    {
        this.itemname = item.itemname;
        this.type = item.type;
        this.information = item.information;
        this.itemObj = item.itemObj;
        this.itemIcon = item.itemIcon;
    }
}

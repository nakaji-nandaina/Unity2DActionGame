using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "CreateQuest")]
public class Quest : ScriptableObject
{
    public enum Type
    {
        Repeatable,
        Key,
    }

    public String questName;
    public Type questType;
    [Multiline(10)]
    public String information;
    public String client;
    public int money;
    public List<RewardItems> rewardItems;
}

[Serializable]
public class RewardItems
{
    public Item item;
    public int num;
}

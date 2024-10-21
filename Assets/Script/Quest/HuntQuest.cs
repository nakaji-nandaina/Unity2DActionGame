using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "CreateQuest/HuntQuest")]

public class HuntQuest : Quest
{
    public List<HuntEnemys> huntEnemys;
}
[Serializable]
public class HuntEnemys
{
    public Enemy enemy;
    public int num;
    [HideInInspector]
    public int huntednum=0;
}

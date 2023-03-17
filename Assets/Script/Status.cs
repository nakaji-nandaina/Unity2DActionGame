using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Status", menuName = "CreateStatus")]
public class Status : ScriptableObject
{
    public int Attack;
    public int Hp;
    public int NextXP;
}

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]

public class Recipe : ScriptableObject
{
    public List<material> materials;
}
[Serializable]
public class material
{
    public Item item;
    public int num;
}
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "WeaponRecipe", menuName = "CreateRecipe/WeaponRecipe")]

public class WeaponRecipe : Recipe
{
    public WeaponData CraftedWeapon;
    public int cost;
}


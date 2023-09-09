using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Pouch", menuName = "Pouch System")]
public class WeaponPouch : ScriptableObject
{
    public int max = 10;
    public List<WeaponData> Pouch = new List<WeaponData>();
    public void AddWeapon(WeaponData newWeapon)
    {
        if (Pouch.Count >= max) return;
        Pouch.Add(newWeapon);
    }
    public void DelWeapon(int id)
    {
        if (Pouch.Count == 1) return;
        if (Pouch.Count < id + 1) return;
        Pouch.RemoveAt(id);
    }

    public void SetInitiate(List<int> ids, DataBase dataBase)
    {
        Pouch = new List<WeaponData>();
        for (int i = 0; i < ids.Count; i++)
        {
            WeaponData weaponData = dataBase.GetWeaponData(ids[i]);
            AddWeapon(weaponData);
        }
    }
}

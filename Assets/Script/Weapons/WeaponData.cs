using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateWeapon")]
public class WeaponData : ScriptableObject
{
    public String Name;
    //攻撃の飛んでいく方法
    public bool delay;
    public bool chase;
    public bool penetrate;
    //武器の画像
    public Sprite ImgWeapon;
    public Sprite Icon;
    //武器のステータス
    public int At;
    public float KbForce;
    public float Speed;
    public float DisTime;
    public float DestTime;
    //効果音
    public AudioClip ShotSound;
    public AudioClip HitSound;
    //武器のオブジェクト
    public GameObject shot;
    
}

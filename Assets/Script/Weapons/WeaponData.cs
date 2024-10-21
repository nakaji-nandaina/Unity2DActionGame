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
    public bool cursor;
    public bool dasei;
    public bool kinsetu;
    public bool yumi;
    public bool bakuhatu;
    //長押しによる武器の射出
    public bool nagaoshi;
    //武器の画像
    public Sprite ImgWeapon;
    public Sprite Icon;
    //武器のステータス
    public int At;
    public float KbForce;
    public float Speed;
    public float DisTime;
    public float DestTime;
    public float AnimTime=1;
    //効果音
    public AudioClip ShotSound;
    public AudioClip HitSound;
    //武器のオブジェクト
    public GameObject shot;
    //武器の説明
    [Multiline(5)]
    public String Explane;
    
}

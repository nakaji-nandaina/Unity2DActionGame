using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CreateWeapon")]
public class WeaponData : ScriptableObject
{
    public String Name;
    //�U���̔��ł������@
    public bool delay;
    public bool chase;
    public bool penetrate;
    //����̉摜
    public Sprite ImgWeapon;
    public Sprite Icon;
    //����̃X�e�[�^�X
    public int At;
    public float KbForce;
    public float Speed;
    public float DisTime;
    public float DestTime;
    //���ʉ�
    public AudioClip ShotSound;
    public AudioClip HitSound;
    //����̃I�u�W�F�N�g
    public GameObject shot;
    
}

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
    public bool cursor;
    public bool dasei;
    public bool kinsetu;
    public bool yumi;
    public bool bakuhatu;
    //�������ɂ�镐��̎ˏo
    public bool nagaoshi;
    //����̉摜
    public Sprite ImgWeapon;
    public Sprite Icon;
    //����̃X�e�[�^�X
    public int At;
    public float KbForce;
    public float Speed;
    public float DisTime;
    public float DestTime;
    public float AnimTime=1;
    //���ʉ�
    public AudioClip ShotSound;
    public AudioClip HitSound;
    //����̃I�u�W�F�N�g
    public GameObject shot;
    //����̐���
    [Multiline(5)]
    public String Explane;
    
}

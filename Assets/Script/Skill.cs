using System;
using UnityEngine;



[Serializable]
[CreateAssetMenu(fileName = "Skill", menuName = "CreateSkill")]
public class Skill : ScriptableObject
{

    public enum EffectedStatus
    {
        Attack,
        HP,
        Extra,
    }

    public enum HowToEffect
    {
        Plus,
        Times,
        Minus,
        divide,
    }

    public string skillName;
    [Header("�X�L�����^�������(�X�e�[�^�X�␳or���̑�)")]
    public EffectedStatus effectedStatus;
    [Header("�X�e�[�^�X�␳�̏ꍇ�̌�������")]
    public HowToEffect howtoEffect;
    [Header("�����X�e�[�^�X�␳��")]
    public float Firstnum;
    [Header("1���x��������̃X�e�[�^�X�␳�l�㏸��")]
    public float Lvnum;
    [Multiline(10)]
    [Header("�X�L���̊T�v")]
    public string Info;
    [Header("���̑��̏ꍇ�̃X�L������(�֐���)")]
    public string Extra;


    public Skill(Skill skill)
    {
        this.skillName = skill.skillName;
        this.effectedStatus = skill.effectedStatus;
        this.howtoEffect = skill.howtoEffect;
        this.Firstnum = skill.Firstnum;
        this.Lvnum = skill.Lvnum;
        this.Info = skill.Info;
        this.Extra = skill.Extra;
    }

}

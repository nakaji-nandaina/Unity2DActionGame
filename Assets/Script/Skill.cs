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
    [Header("スキルが与える効果(ステータス補正orその他)")]
    public EffectedStatus effectedStatus;
    [Header("ステータス補正の場合の効かせ方")]
    public HowToEffect howtoEffect;
    [Header("初期ステータス補正量")]
    public float Firstnum;
    [Header("1レベル当たりのステータス補正値上昇量")]
    public float Lvnum;
    [Multiline(10)]
    [Header("スキルの概要")]
    public string Info;
    [Header("その他の場合のスキル効果(関数名)")]
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

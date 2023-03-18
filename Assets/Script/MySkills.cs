using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillContainer", menuName = "Skill System/MySkillContainer")]
public class MySkills : ScriptableObject
{
    [Header("ŠƒXƒLƒ‹")]
    public List<SkillSlot> skillContainer = new List<SkillSlot>();

    public int AddSkill(Skill _skill, int _Lv)
    {
        bool hasSkill = false;
        for (int i = 0; i < skillContainer.Count; i++)
        {
            if (skillContainer[i].skill == _skill)
            {
                skillContainer[i].AddLv(_Lv);
                hasSkill = true;
                break;
            }
        }
        if (!hasSkill)
        {
            skillContainer.Add(new SkillSlot(_skill, _Lv));

        }
        return _Lv;
    }

    public void SetInitiate(List<int> ids, List<int> Lvs, DataBase dataBase)
    {
        for (int i = 0; i < ids.Count; i++)
        {
            Skill skill = dataBase.GetSkillData(ids[i]);
            AddSkill(skill, Lvs[i]);
        }
    }

    public float[] culculateSkillEffects()
    {
        float[] status = new float[4];
        //[0]:plus,[1]:times
        float[] At = new float[2];
        float[] Hp = new float[2];
        
        for(int i = 0; i < skillContainer.Count; i++)
        {
            Skill skill = skillContainer[i].skill;
            int Lv = skillContainer[i].Lv;
            switch (skill.effectedStatus)
            {
                case Skill.EffectedStatus.Attack:
                    At[0] += culculateSkillEffect(skill, Lv)[0];
                    At[1] += culculateSkillEffect(skill, Lv)[1];
                    break;
                case Skill.EffectedStatus.HP:
                    Hp[0] += culculateSkillEffect(skill, Lv)[0];
                    Hp[1] += culculateSkillEffect(skill, Lv)[1];
                    break;
            }
        }
        status[0] = At[0];
        status[1] = At[1];
        status[2] = Hp[0];
        status[3] = Hp[1];
        return status;
    }

    public float[] culculateSkillEffect(Skill skill,int Lv)
    {
        float[] num = new float[2];
        switch (skill.howtoEffect)
        {
            case Skill.HowToEffect.Plus:
                num[0] += skill.Firstnum+skill.Lvnum*Lv;
                break;

            case Skill.HowToEffect.Times:
                num[1] += skill.Firstnum + skill.Lvnum * Lv;
                break;
        }
        return num;
    }

}

[System.Serializable]
public class SkillSlot
{
    public Skill skill;
    public int Lv;
    public SkillSlot(Skill _skill, int _Lv)
    {
        skill = _skill;
        Lv = _Lv;
    }
    public void AddLv(int Value)
    {
        Lv += Value;
    }
}

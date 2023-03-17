using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SkillContainer", menuName = "Skill System/MySkillContainer")]
public class MySkills : ScriptableObject
{
    [Header("èäéùÉXÉLÉã")]
    public List<SkillSlot> skillContainer = new List<SkillSlot>();
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

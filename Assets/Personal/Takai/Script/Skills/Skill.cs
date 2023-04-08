using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour, ISkillBase
{
    private ISkillBase _skillBaseImplementation;
    public int SkillId { get; set; }
    public string SkillName { get; set; }
    public int RequiredSP { get; set; }
    public int Damage { get; set; }
    
    public WeponType Type { get; set; }
    
    public void SetUp()
    {
        SkillId = 1;
        SkillName = "Skill1";
        RequiredSP = 3;
        Damage = 5;
        Type = WeponType.LargeSword;
    }

    public void SendSkill()
    {
        SkillAcquisition skillAcquisition = new SkillAcquisition();

        Skill skill = new Skill();
        skill.SetUp();

        skillAcquisition.AddSkill(skill);
    }

    public void UseSkill()
    {
    }

    public void SkillEffect()
    {
        
    }
}
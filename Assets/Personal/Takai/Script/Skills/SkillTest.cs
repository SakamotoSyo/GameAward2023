using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill :  ISkillBase
{
    private ISkillBase _skillBaseImplementation;
    public int SkillId { get; set; }
    public string SkillName { get; set; }
    public int RequiredSP { get; set; }
    public int Damage { get; set; }
    
    public WeaponType Type { get; set; }
    
    public void SetUp()
    {
        SkillId = 1;
        SkillName = "Skill1";
        RequiredSP = 0;
        Damage = 5;
        Type = WeaponType.LargeSword;
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
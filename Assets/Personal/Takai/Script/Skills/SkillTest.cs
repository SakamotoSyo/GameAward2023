using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour, ISkillBase
{
    public int SkillId { get; set; }
    public string SkillName { get; set; }
    public int RequiredSP { get; set; }
    public int Damage { get; set; }
    public WeaponType Type { get; set; }
    public SkillAcquisition SkillAcquisition { get; set; }
    private void Start()
    {
        SendSkill();
    }

    public void SetUp()
    {
        SkillId = 1;
        SkillName = "Skill1";
        RequiredSP = 0;
        Damage = 5;
        Type = WeaponType.GreatSword;
    }

    public void SendSkill()
    {
        SkillAcquisition = FindObjectOfType<SkillAcquisition>();

        SkillTest mySkill = new SkillTest();
        mySkill.SetUp();

        SkillAcquisition.AddSkill(mySkill);
    }

    public void UseSkill()
    {
        Debug.Log("Use Skill");
    }

    public void SkillEffect()
    {
        Debug.Log("Skill Effect");
    }
}
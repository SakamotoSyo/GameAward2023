using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTest : MonoBehaviour, ISkillBase
{
    public string SkillName { get; set; }
    public int Damage { get; set; }
    public WeaponType Type { get; set; }
    public OreRarity Rarity { get; set; }
    public PlayerSkillDataManagement playerSkillDataManagement { get; set; }
    private void Start()
    {
        SendSkill();
    }

    public void SetUp()
    {
        SkillName = "TestSkill";
        Damage = 5;
        Type = WeaponType.GreatSword;
        Rarity = OreRarity.Normal;
    }

    public void SendSkill()
    {
        playerSkillDataManagement = FindObjectOfType<PlayerSkillDataManagement>();

        SkillTest mySkill = new SkillTest();
        mySkill.SetUp();

        playerSkillDataManagement.AddSkill(mySkill);
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
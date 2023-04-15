using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class SkillTest : MonoBehaviour, ISkillBase 
{
    public string SkillName { get; set; }
    public int Damage { get; set; }
    public WeaponType Weapon { get; set; }
    public OreRarity Rarity { get; set; }
    public SkillType Type  { get; set; }
    public PlayerSkillDataManagement playerSkillDataManagement { get; set; }

    private Animator _anim;
    private void Start()
    {
        SendSkill();
    }

    public void SetUp()
    {
        SkillName = "TestSkill";
        Damage = 5;
        Weapon = WeaponType.GreatSword;
        Rarity = OreRarity.Normal;
        Type = SkillType.Skill;
        _anim = GetComponent<Animator>();
    }

    public void SendSkill()
    {
        playerSkillDataManagement = FindObjectOfType<PlayerSkillDataManagement>();

        //SkillTest mySkill = new SkillTest();
        SetUp();

        playerSkillDataManagement.AddSkill(this);
    }

    public async UniTask UseSkill()
    {
        Debug.Log("Use Skill");
        await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Debug.Log("Anim End");
    }

    public void SkillEffect()
    {
        Debug.Log("Skill Effect");
    }
}
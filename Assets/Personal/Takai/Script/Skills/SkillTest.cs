using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public  class SkillTest : SkillBase 
{
    public string SkillName { get; set; }
    public int Damage { get; set; }
    public WeaponType Weapon { get; set; }
    public OreRarity Rarity { get; set; }
    public SkillType Type  { get; set; }
    
    private Animator _anim;

    public override async UniTask UseSkill()
    {
        Debug.Log("Use Skill");
        _anim = GetComponent<Animator>();
        await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        Debug.Log("Skill Effect");
    }
}
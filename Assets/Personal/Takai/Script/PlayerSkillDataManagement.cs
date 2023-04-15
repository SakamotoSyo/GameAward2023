using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSkillDataManagement : MonoBehaviour
{
    private List<SkillBase> _skills = new List<SkillBase>();
    public IReadOnlyList<SkillBase> PlayerSkillList => _skills;

    private void Start()
    {
        SkillBase[] skillPrefabs = Resources.LoadAll<SkillBase>("Skills");

        foreach (var skill in skillPrefabs)
        {
           var sObj = Instantiate(skill, transform);
            _skills.Add(sObj);
        }
    }

    public void OnCall()
    {
        Debug.Log(OnSkillCall(WeaponType.GreatSword,OreRarity.Normal));

        OnSkillCall(WeaponType.GreatSword, OreRarity.Normal).UseSkill().Forget();
    }
    
    public SkillBase OnSkillCall(WeaponType weapon, OreRarity rarity)
    {
        List<SkillBase> skills = new List<SkillBase>();
        
        foreach (var s in _skills)
        {
            if (s.Weapon == weapon && s.Rarity == rarity)
            {
                skills.Add(s);
            }
        }
        
        int n = Random.Range(0, skills.Count);
        
       return skills[n];
    }
}
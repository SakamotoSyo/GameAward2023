using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSkillDataManagement : MonoBehaviour
{
    private List<ISkillBase> _skills = new List<ISkillBase>();
    public IReadOnlyList<ISkillBase> PlayerSkillList => _skills;
    public void AddSkill(ISkillBase skill)
    {
        if (_skills == null)
        {
            _skills = new List<ISkillBase>();
        }

        _skills.Add(skill);
    }

    public ISkillBase OnSkillCall(WeaponType type, OreRarity rarity)
    {
        List<ISkillBase> skills = new List<ISkillBase>();
        
        foreach (var s in _skills)
        {
            if (s.Type == type && s.Rarity == rarity)
            {
                skills.Add(s);
            }
        }
        
        int n = Random.Range(0, skills.Count);
        
       return skills[n];
    }
}
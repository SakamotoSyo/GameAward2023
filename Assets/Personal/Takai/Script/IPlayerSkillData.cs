using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPlayerSkillData : MonoBehaviour
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

    public void OnUse()
    {
        _skills[0].UseSkill();
    }
}

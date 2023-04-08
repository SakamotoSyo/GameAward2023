using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPlayerSkillData : MonoBehaviour
{
    private List<ISkillBase> _skills;

    public void AddSkill(ISkillBase skill)
    {
        if (_skills == null)
        {
            _skills = new List<ISkillBase>();
        }

        _skills.Add(skill);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAcquisition : MonoBehaviour
{
    private List<ISkillBase> _skills = new List<ISkillBase>();
    public void AddSkill(ISkillBase skill)
    {
        if (_skills == null)
        {
            _skills = new List<ISkillBase>();
        }

        _skills.Add(skill);
    }

    public void OnSkillRead(String name)
    {
        Debug.Log(_skills.Count);
        
        WeaponType type = (WeaponType)Enum.Parse(typeof(WeaponType), name);
        
        foreach (var s in _skills)
        {
            if (s.Type != type)
            {
                return;
            }

            int sp = s.RequiredSP;

            OnSkillGet(s);
        }
    }

    void OnSkillGet(ISkillBase skill)
    {
        IPlayerSkillData playerSkillData = FindObjectOfType<IPlayerSkillData>();

        playerSkillData.AddSkill(skill);
    }
}
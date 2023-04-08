using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAcquisition
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
    
    public void OnSkillRead(int lv,WeponType type)
    {
        foreach (var s in _skills)
        {
            if (s.Type != type) {return;}
            
            int sp = s.RequiredSP;

            if (sp <= lv)
            {
                OnSkillGet(s);
            }
        }
    }

    void OnSkillGet(ISkillBase skill)
    {
        IPlayerSkillData playerSkillData = new IPlayerSkillData();
        
        playerSkillData.AddSkill(skill);
    }
}
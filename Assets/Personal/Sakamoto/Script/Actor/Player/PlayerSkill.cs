using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSkill
{
    [SerializeField] private string _skillName;
    public SkillBase SpecialAttack => _specialAttack;
    public SkillBase[] PlayerSkillArray => _skillArray;
    [Tooltip("•KŽE‹Z")]
    private SkillBase _specialAttack;
    private SkillBase[] _skillArray = new SkillBase[2];
    private SkillDataManagement _skillDataManagement;
    private bool _isSkillDebug;

    public void Init(SkillDataManagement skillManagement) 
    {
        _skillDataManagement = skillManagement;
        if (_isSkillDebug) 
        {
            _skillArray[0] = _skillDataManagement.SearchSkill();
        }
    }

    public void SaveSkill(PlayerSaveData saveData) 
    {
        saveData.SpecialAttack = _specialAttack;
        saveData.PlayerSkillArray = _skillArray;
    }

    public void ChangeSpecialSkill(SkillBase skill) 
    {
        _specialAttack = skill;
    }

    public void LoadSkill(PlayerSaveData playerSaveData) 
    {
        _skillArray = playerSaveData.PlayerSkillArray;
        Debug.Log(_skillArray.Length);
        _specialAttack = playerSaveData.SpecialAttack;
    }

    public bool AddSpecialSkill(SkillBase skill) 
    {
        if (!_specialAttack) 
        {
            _specialAttack = skill;
            return true;
        }
        return false;
    }

    public bool AddSkillJudge(SkillBase skill) 
    {
        for (int i = 0; i < _skillArray.Length; i++) 
        {
            if (_skillArray[i] == null) 
            {
                _skillArray[i] = skill;
                return true;
            }
        }

        return false;
    }
}

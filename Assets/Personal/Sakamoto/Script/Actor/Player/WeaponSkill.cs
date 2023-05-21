using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponSkill
{
    public SkillBase SpecialAttack => _specialAttack;
    public SkillBase[] WeaponSkillArray => _skillArray;
    public SkillDataManagement SkillDataManagement => _skillDataManagement;
    [Tooltip("必殺技")]
    private SkillBase _specialAttack;
    private SkillBase[] _skillArray = new SkillBase[2];
    private SkillDataManagement _skillDataManagement;
    private WeaponType _weaponType;
    [SerializeField] private bool _isSkillDebug;

    public void Init(SkillDataManagement skillManagement) 
    {
        _skillDataManagement = skillManagement;
        if (_isSkillDebug) 
        {
            _skillArray[0] = _skillDataManagement.SearchSkill();
        }
    }

    public void SetWeaponType(WeaponType weaponType) 
    {
        _weaponType = weaponType;
    }

    public void ChangeSpecialSkill(SkillBase skill) 
    {
        _specialAttack = skill;
    }

    public SkillBase CounterCheck() 
    {
        //return _skillDataManagement.CounterCheck();
        return default;
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
            if (_skillArray[i] == null && skill.Weapon == _weaponType) 
            {
                _skillArray[i] = skill;
                return true;
            }
        }

        return false;
    }

    public int GetSkillData() 
    {
        int value = 0;
        for(int i = 0; i < _skillArray.Length; i++) 
        {
            if (_skillArray[i] != null) 
            {
                value++;
            }
        }
        return value;
    }

    /// <summary>
    /// Eipcスキルをチェックしてあれば処理する
    /// </summary>
    public void EpicSkillCheck() 
    {
        for(int i = 0; i < _skillArray.Length; i++) 
        {
            if (_skillArray[i].Type == SkillType.Epic) 
            {
                _skillDataManagement.OnSkillUse(ActorAttackType.Player, _skillArray[i].name);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponSkill
{
    public string SpecialAttack => _specialAttack;
    public string[] WeaponSkillArray => _skillArray;
    public SkillDataManagement SkillDataManagement => _skillDataManagement;
    [Tooltip("必殺技")]
    private string _specialAttack;
    [Header("スキルの名前を設定")]
    [SerializeField] private string[] _skillArray = new string[2];
    private SkillDataManagement _skillDataManagement;
    private WeaponType _weaponType;

    public void Init(SkillDataManagement skillManagement) 
    {
        _skillDataManagement = skillManagement;
    }

    public void SetWeaponType(WeaponType weaponType) 
    {
        _weaponType = weaponType;
    }

    public void ChangeSpecialSkill(string skillName) 
    {
        _specialAttack = skillName;
    }

    public SkillBase CounterCheck() 
    {
        //return _skillDataManagement.CounterCheck();
        return default;
    }

    public bool AddSpecialSkill(string specialAttack) 
    {
        if (_specialAttack == "") 
        {
            _specialAttack = specialAttack;
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
                _skillArray[i] = skill.SkillName;
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
            var skill = _skillDataManagement.SearchSkill(_skillArray[i]);
            if (skill.Type == SkillType.Epic) 
            {
                _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill.name);
            }
        }
    }
}

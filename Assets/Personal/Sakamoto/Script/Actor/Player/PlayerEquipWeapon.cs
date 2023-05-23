using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

public class PlayerEquipWeapon
{
    public IReactiveProperty<float> OffensivePower => _offensivePower;
    public IReactiveProperty<float> WeaponWeight => _weaponWeight;
    public IReactiveProperty<float> CriticalRate => _criticalRate;
    public IReactiveProperty<float> MaxDurable => _maxDurable;
    public IReactiveProperty<float> CurrentDurable => _currentDurable;
    public WeaponData.AttributeType Attribute => _attribute;
    public WeaponType WeaponType => _weponType;
    public WeaponSkill WeaponSkill => _weaponSkill;
    public int WeaponNum => _weaponNum;
    public bool IsEpicSkill1 => _isEpicSkill1;
    public bool IsEpicSkill2 => _isEpicSkill2;
    public bool IsEpicSkill3 => _isEpicSkill3;

    private ReactiveProperty<float> _offensivePower = new();
    private ReactiveProperty<float> _weaponWeight = new();
    private ReactiveProperty<float> _criticalRate = new();
    private ReactiveProperty<float> _maxDurable = new();
    private ReactiveProperty<float> _currentDurable = new();
    private bool _isEpicSkill1 = false, _isEpicSkill2 = false, _isEpicSkill3 = false;
    private WeaponSkill _weaponSkill;
    private WeaponData.AttributeType _attribute;
    private WeaponType _weponType;
    [Tooltip("何番目に持っている武器か")]
    private int _weaponNum;
    private SkillDataManagement _skillDataManagement;

    public void Init(SkillDataManagement skillDataManagement)
    {
        _skillDataManagement = skillDataManagement;
        EpicSkillCheck();
    }

    public float GetPowerPram()
    {
        if (_isEpicSkill2)
        {
            return _offensivePower.Value * 1.4f;
        }
        return _offensivePower.Value;
    }

    public float GetWeightPram()
    {
        if (_isEpicSkill2)
        {
            return _criticalRate.Value * 1.4f;
        }
        return _criticalRate.Value;
    }

    public float GetCriticalPram()
    {
        if (_isEpicSkill2)
        {
            return _criticalRate.Value * 1.4f;
        }
        return _criticalRate.Value;
    }

    public virtual void AddDamage(float damage)
    {
        if (_isEpicSkill2)
        {
            ChangeCurrentDurable(damage * -1 * 2);
        }
        else
        {
            ChangeCurrentDurable(damage * -1);
        }
    }

    /// <summary>
    /// 攻撃でダウンするか判定する
    /// 防御力とかが増えた場合こんな関数があると楽
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool DownJudge(float damage)
    {
        if (_isEpicSkill2)
        {
            return 0 < _currentDurable.Value - damage * 2;
        }
        return 0 < _currentDurable.Value - damage;
    }

    /// <summary>
    /// 武器を変更する際に使用する関数
    /// </summary>
    public void ChangeWeapon(WeaponData weaponData, int arrayNum)
    {
        _offensivePower.Value = weaponData.OffensivePower;
        _weaponWeight.Value = weaponData.WeaponWeight;
        _criticalRate.Value = weaponData.CriticalRate;
        _maxDurable.Value = weaponData.MaxDurable;
        ChangeCurrentDurable(weaponData.CurrentDurable);
        _attribute = weaponData.Attribute;
        _weponType = weaponData.WeaponType;
        _weaponNum = arrayNum;
        _weaponSkill = weaponData.WeaponSkill;
        if (_skillDataManagement)
        {
            EpicSkillCheck();
        }
    }

    /// <summary>
    /// 武器を入れ替えたときなどにEpicSkillがあるかどうか確認して発動する
    /// </summary>
    public void EpicSkillCheck()
    {
        for (int i = 0; i < _weaponSkill.WeaponSkillArray.Length; i++)
        {
            var skill = _skillDataManagement.SearchSkill(_weaponSkill.WeaponSkillArray[i]);
            if (skill != null && skill.Type == SkillType.Epic)
            {
                _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill.name).Forget();
                Debug.Log("Epic発動");
            }
        }
    }

    public void FluctuationStatus(FluctuationStatusClass fluctuation)
    {
        _offensivePower.Value += fluctuation.OffensivePower;
        _weaponWeight.Value += fluctuation.WeaponWeight;
        _criticalRate.Value += fluctuation.CriticalRate;
        _maxDurable.Value += fluctuation.MaxDurable;
        ChangeCurrentDurable(fluctuation.CurrentDurable);
    }

    public void EpicSkill1()
    {
        _maxDurable.Value = 5;
        _currentDurable.Value = 5;
        _isEpicSkill1 = true;
    }

    public void EpicSkill2()
    {
        _isEpicSkill2 = true;
    }

    private void ChangeCurrentDurable(float num)
    {
        _currentDurable.Value += num;
    }
}

public class FluctuationStatusClass
{
    public float OffensivePower;
    public float WeaponWeight;
    public float CriticalRate;
    public float MaxDurable;
    public float CurrentDurable;

    public FluctuationStatusClass(float offensivePower, float weaponWeight, float criticalRate, float maxDurable, float currentDurable)
    {
        OffensivePower = offensivePower;
        WeaponWeight = weaponWeight;
        CriticalRate = criticalRate;
        MaxDurable = maxDurable;
        CurrentDurable = currentDurable;
    }
}


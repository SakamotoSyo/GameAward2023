using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class EquipEnemyWeapon
{

    /// <summary>武器の攻撃力</summary>
    private float _offensivePower;

    /// <summary>武器の攻撃力のプロパティ</summary>
    public float OffensivePower => _offensivePower;

    /// <summary>現在の武器の攻撃力</summary>
    private float _currentOffensivePower;

    /// <summary>現在の武器の攻撃力のプロパティ</summary>
    public float CurrentOffensivePower { get => _currentOffensivePower; set => _currentOffensivePower = value; }

    /// <summary>武器のクリティカル率</summary>
    private float _criticalRate;

    /// <summary>武器のクリティカル率のプロパティ</summary>
    public float CriticalRate => _criticalRate;

    /// <summary>現在の武器のクリティカル率</summary>
    private float _currentCriticalRate;

    /// <summary>現在の武器のクリティカル率のプロパティ</summary>
    public float CurrentCriticalRate { get => _currentCriticalRate; set => _currentCriticalRate = value; }

    /// <summary>武器の最大耐久値</summary>
    private ReactiveProperty<float> _maxDurable = new();

    public IReactiveProperty<float> MaxDurable => _maxDurable;

    /// <summary>現在の武器の耐久値</summary>
    private ReactiveProperty<float> _currentDurable = new();

    /// <summary>現在の武器の耐久値のプロパティ</summary>
    public IReactiveProperty<float> CurrentDurable => _currentDurable;

    /// <summary>武器の重さ</summary>
    private float _weaponWeight;

    /// <summary>武器の重さのプロパティ</summary>
    public float WeaponWeight => _weaponWeight;

    /// <summary>現在の武器の重さ</summary>
    private float _currentWeaponWeight;

    /// <summary>現在の武器の重さのプロパティ</summary>
    public float CurrentWeaponWeight { get => _currentWeaponWeight; set => _currentWeaponWeight = value; }

    /// <summary>武器の種類</summary>
    private ReactiveProperty<WeaponType> _weaponType;

    public WeaponType WeaponType => _weaponType.Value;
    public IObservable<WeaponType> WeaponTypeOb => _weaponType;

    private WeaponSkill _weaponSkill;

    public WeaponSkill WeaponSkill => _weaponSkill;

    private int _breakCount = 0;

    public int WeaponBreakCount => _breakCount;

    /// <summary>
    /// 武器へのダメージ
    /// </summary>
    public bool AddDamage(int damage, float criticalRate)
    {
        int critical = Random.Range(0, 100);
        if(critical >= criticalRate)
        {
            Debug.Log("クリティカル");
            damage = (int)((float)damage * 1.3f);
            _currentDurable.Value -= damage;
            return true;
        }
        _currentDurable.Value -= damage;
        return false;
    }

    public bool IsWeaponBreak()
    {
        if (_currentDurable.Value <= 0)
        {
            _breakCount++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 装備している武器を交代する
    /// </summary>
    public void ChangeWeapon(WeaponData weaponData)
    {
        _currentOffensivePower = weaponData.OffensivePower;
        _currentCriticalRate = weaponData.CriticalRate;
        _maxDurable.Value = weaponData.MaxDurable;
        _currentWeaponWeight = weaponData.WeaponWeight;
        _currentDurable.Value = weaponData.CurrentDurable;
        _weaponType.Value = weaponData.WeaponType;
        _weaponSkill = weaponData.WeaponSkill;

        _offensivePower = weaponData.OffensivePower;
        _criticalRate = weaponData.CriticalRate;
        _weaponWeight = weaponData.WeaponWeight;
    }

    public void FluctuationStatus(FluctuationStatusClass fluctuation)
    {
        _currentOffensivePower += fluctuation.OffensivePower;
        _currentWeaponWeight += fluctuation.WeaponWeight;
        _currentCriticalRate += fluctuation.CriticalRate;
        _maxDurable.Value += fluctuation.MaxDurable;
        _currentDurable.Value += fluctuation.CurrentDurable;
    }

}

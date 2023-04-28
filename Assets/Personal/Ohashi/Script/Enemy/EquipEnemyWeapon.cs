using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EquipEnemyWeapon
{

    /// <summary>武器の攻撃力</summary>
    private float _offensivePower;

    /// <summary>武器の攻撃力のプロパティ</summary>
    public float OffensivePower { get => _offensivePower; set => _offensivePower = value; }

    /// <summary>武器のクリティカル率</summary>
    private float _criticalRate;

    /// <summary>武器のクリティカル率のプロパティ</summary>
    public float CriticalRate { get => _criticalRate; set => _criticalRate = value; }

    /// <summary>武器の最大耐久値</summary>
    private float _maxDurable;

    /// <summary>武器の現在の耐久値</summary>
    private ReactiveProperty<float> _currentDurable = new();

    /// <summary>武器の現在の耐久値のプロパティ</summary>
    public IReactiveProperty<float> CurrentDurable => _currentDurable;

    /// <summary>武器の重さ</summary>
    private float _weaponWeight;

    /// <summary>武器の重さのプロパティ</summary>
    public float WeaponWeight { get => _weaponWeight; set => _weaponWeight = value; }

    /// <summary>武器の種類</summary>
    private WeaponType _weaponType;

    private int _breakCount = 0;

    public int WeaponBreakCount => _breakCount;

    /// <summary>
    /// 武器へのダメージ
    /// </summary>
    public void AddDamage(float damage)
    {
        _currentDurable.Value -= damage;

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
        _offensivePower = weaponData.OffensivePower;
        _criticalRate = weaponData.CriticalRate;
        _maxDurable = weaponData.MaxDurable;
        _weaponWeight = weaponData.WeaponWeight;
        _currentDurable.Value = weaponData.CurrentDurable;
        _weaponType = weaponData.WeaponType;
    }

}

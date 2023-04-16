using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EquipEnemyWeapon
{

    /// <summary>武器の攻撃力</summary>
    private float _offensivePower;

    /// <summary>武器のクリティカル率</summary>
    private float _criticalRate;

    /// <summary>武器の最大耐久値</summary>
    private float _maxDurable;

    /// <summary>武器の現在の耐久値</summary>
    private ReactiveProperty<float> _currentDurable = new();

    public IReactiveProperty<float> CurrentDurable => _currentDurable;

    /// <summary>武器の重さ</summary>
    private float _weaponWeight;

    public float WeaponWeight => _weaponWeight;

    /// <summary>
    /// 武器へのダメージ
    /// </summary>
    public void AddDamage(float damage)
    {
        _currentDurable.Value -= damage;
    }

    /// <summary>
    /// 持っている武器を交代する
    /// </summary>
    public void ChangeWeapon(WeaponData weaponData)
    {
        _offensivePower = weaponData.OffensivePower;
        _criticalRate = weaponData.CriticalRate;
        _maxDurable = weaponData.MaxDurable;
        _weaponWeight = weaponData.WeaponWeight;
        _currentDurable.Value = weaponData.CurrentDurable;
    }

}

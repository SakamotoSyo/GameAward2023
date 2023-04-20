using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public int WeaponNum => _weaponNum;

    private ReactiveProperty<float> _offensivePower = new();
    private ReactiveProperty<float> _weaponWeight = new();
    private ReactiveProperty<float> _criticalRate = new();
    private ReactiveProperty<float> _maxDurable = new();
    private ReactiveProperty<float> _currentDurable = new();
    private WeaponData.AttributeType _attribute;
    private WeaponType _weponType;
    [Tooltip("何番目に持っている武器か")]
    private int _weaponNum;

    public virtual void AddDamage(float damage)
    {
        _currentDurable.Value -= damage;
    }

    /// <summary>
    /// 攻撃でダウンするか判定する
    /// 防御力とかが増えた場合こんな関数があると楽
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool DownJudge(float damage)
    {
        return 0 < _currentDurable.Value - damage;
    }

    /// <summary>
    /// 武器を変更する際に使用する関数
    /// </summary>
    public void ChangeWeapon(WeaponData weaponData) 
    {
        _offensivePower.Value = weaponData.OffensivePower;
        _weaponWeight.Value = weaponData.WeaponWeight;
        _criticalRate.Value = weaponData.CriticalRate;
        _maxDurable.Value = weaponData.MaxDurable;
        _currentDurable.Value = weaponData.CurrentDurable;
        _attribute = weaponData.Attribute;
        _weponType = weaponData.WeaponType;
    }
}

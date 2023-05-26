using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class EnemyStatus
{
    private WeaponData[] _weaponDatas;

    public WeaponData[] WeaponDates => _weaponDatas;

    private EquipEnemyWeapon _epicWeapon = new();

    public EquipEnemyWeapon EquipWeapon => _epicWeapon;

    private SkillBase[] _skills = new SkillBase[2];

    private SkillBase _specialSkill;

    private bool _isStan = false;

    public bool IsStan { get => _isStan; set => _isStan = value; }

    private bool _isBoss = false;

    public bool IsBoss { get => _isBoss; set => _isBoss = value; }

    /// <summary>
    /// 武器の配列の更新
    /// </summary>
    public void SetWeaponDates(EnemyData enemyData)
    {
        _weaponDatas = enemyData.WeaponDatas;
        _epicWeapon.ChangeWeapon(_weaponDatas[0]);
    }

    public bool IsWeaponsAllBrek()
    {
        if(_epicWeapon.WeaponBreakCount >= _weaponDatas.Length)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// どの武器に変更するかを指定する
    /// </summary>
    public async UniTask EquipChangeWeapon()
    {
        foreach(WeaponData weapon in _weaponDatas)
        {
            if(weapon.CurrentDurable > 0)
            {
                await UniTask.Delay(1);
                _epicWeapon.ChangeWeapon(weapon);
                return;
            }
        }
    }

    public bool IsDebuff()
    {
        if(_epicWeapon.OffensivePower != _epicWeapon.CurrentOffensivePower ||
            _epicWeapon.CriticalRate != _epicWeapon.CurrentCriticalRate ||
            _epicWeapon.WeaponWeight != _epicWeapon.CurrentWeaponWeight)
        {
            return true;
        }

        return false;
    }
}

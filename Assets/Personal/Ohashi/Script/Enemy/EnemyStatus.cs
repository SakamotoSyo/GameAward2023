using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyStatus
{
    private WeaponData[] _weaponDates;

    public WeaponData[] WeaponDates => _weaponDates;

    private EquipEnemyWeapon _epicWeapon = new();

    public EquipEnemyWeapon EquipWeapon => _epicWeapon;

    /// <summary>
    /// 武器の配列の更新
    /// </summary>
    public void SetWeaponDates(EnemyData enemyDate)
    {
        _weaponDates = enemyDate.WeaponDates;
    }

    /// <summary>
    /// どの武器に変更するかを指定する
    /// </summary>
    public void EquipChangeWeapon(int weaponNum)
    {
        _epicWeapon.ChangeWeapon(_weaponDates[weaponNum]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyStatus
{
    private WeaponData[] _weaponDates;

    public WeaponData[] WeaponDates => _weaponDates;

    private EquipEnemyWeapon _epicEnemyWeapon = new();

    public EquipEnemyWeapon EquipEnemyWeapon => _epicEnemyWeapon;

    /// <summary>
    /// 武器の配列の更新
    /// </summary>
    public void SetWeaponDate(WeaponData[] weaponDatas)
    {
        _weaponDates = weaponDatas;
    }

    /// <summary>
    /// どの武器に変更するかを指定する
    /// </summary>
    public void EquipChangeWeapon(int weaponNum)
    {
        _epicEnemyWeapon.ChangeWeapon(_weaponDates[weaponNum]);
    }
}

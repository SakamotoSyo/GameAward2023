using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyStatus
{
    private WeaponData[] _weaponDatas;

    public WeaponData[] WeaponDates => _weaponDatas;

    private EquipEnemyWeapon _epicWeapon = new();

    public EquipEnemyWeapon EquipWeapon => _epicWeapon;

    /// <summary>
    /// 武器の配列の更新
    /// </summary>
    public void SetWeaponDates(EnemyData enemyData)
    {
        _weaponDatas = enemyData.WeaponDates;
    }

    /// <summary>
    /// どの武器に変更するかを指定する
    /// </summary>
    public void EquipChangeWeapon(int weaponNum)
    {
        _epicWeapon.ChangeWeapon(_weaponDatas[weaponNum]);
    }
}

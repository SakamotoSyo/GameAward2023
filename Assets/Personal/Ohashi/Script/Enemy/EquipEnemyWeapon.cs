using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EquipEnemyWeapon
{
    private float _weaponPower;

    private float _maxHp;

    private float _enemySpeed;

    public float EnemySpeed => _enemySpeed;

    private ReactiveProperty<float> _currentHp = new();

    public void AddDamage(float damage)
    {
        _currentHp.Value -= damage;
    }

    public void ChangeWeapon(WeaponData weaponData)
    {

    }

}

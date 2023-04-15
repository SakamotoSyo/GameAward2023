using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EquipEnemyWeapon : MonoBehaviour
{
    [SerializeField]
    private float _weaponPower;

    [SerializeField]
    private float _maxHp;

    [SerializeField]
    private float _enemySpeed;

    private ReactiveProperty<float> _currentHp = new();

    public void AddDamage(float damage)
    {
        _currentHp.Value -= damage;
    }

    public void ChangeWeapon(WeaponData weaponData)
    {

    }

}

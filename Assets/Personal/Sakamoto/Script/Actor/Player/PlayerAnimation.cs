using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class PlayerAnimation
{
    [SerializeField] private Animator _idleAnim;
    [SerializeField] private string[] _idleAnimPram = new string[4];
    private string _spear;
    private string _greatSword;
    private string _hammer;
    private PlayerEquipWeapon _equipWeapon;
    public void Init(PlayerEquipWeapon equipWeapon)
    {
        _equipWeapon = equipWeapon;
        WeaponIdle();
    }

    public void WeaponIdle()
    {
        IdleRelease();
        if (_equipWeapon.WeaponType == WeaponType.DualBlades)
        {
            _idleAnim.SetBool(_idleAnimPram[2], true);
        }
        else if (_equipWeapon.WeaponType == WeaponType.Spear)
        {
            _idleAnim.SetBool(_idleAnimPram[0], true);
        }
        else if (_equipWeapon.WeaponType == WeaponType.GreatSword)
        {
            _idleAnim.SetBool(_idleAnimPram[3], true);
        }
        else if (_equipWeapon.WeaponType == WeaponType.Hammer)
        {
            _idleAnim.SetBool(_idleAnimPram[1], true);
        }
    }

    private void IdleRelease() 
    {
        for(int i = 0; i < _idleAnimPram.Length; i++) 
        {
            _idleAnim.SetBool(_idleAnimPram[i], false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class PlayerAnimation
{
    [SerializeField] private Animator _idleAnim;
    [SerializeField] private string[] _idleAnimPram = new string[4];
    private string _spear;
    private string _greatSword;
    private string _hammer;
    private bool _isBattleEnd = false;
    private bool _isDamage = false;
    private PlayerEquipWeapon _equipWeapon;
    public void Init(PlayerEquipWeapon equipWeapon)
    {
        _equipWeapon = equipWeapon;
        WeaponIdle();
    }

    public void Update()
    {
        if (_isBattleEnd) return;
        IdleRelease();
        if (_isDamage) 
        {
            
        }
        else if (_equipWeapon.WeaponType == WeaponType.DualBlades)
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

    public async void Damage() 
    {
        _isDamage = true;
        IdleRelease();
        _idleAnim.SetBool("Damage", _isDamage);
        await UniTask.WaitUntil(() => _idleAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
        _isDamage = false;
        _idleAnim.SetBool("Damage", _isDamage);
    }

    public void Victory() 
    {
        _isBattleEnd = true;
        IdleRelease();
        _idleAnim.SetBool("Victory", true);
    }

    public async UniTask Lose() 
    {
        _isBattleEnd = true;
        IdleRelease();
        _idleAnim.SetBool("Lose", true);
        await UniTask.WaitUntil(() => _idleAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f);
    }

    private void IdleRelease() 
    {
        for(int i = 0; i < _idleAnimPram.Length; i++) 
        {
            _idleAnim.SetBool(_idleAnimPram[i], false);
        }
    }
}
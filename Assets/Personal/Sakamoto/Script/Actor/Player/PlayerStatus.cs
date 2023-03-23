using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using VContainer.Unity;

public class PlayerStatus : ActorStatusBase
{
    public List<ISkillBase> PlayerSkillList => _skillList;
    public WeaponData EquipWepon => _equipWeapon;

    private int _level;
    private int _skillPoint;
    private WeaponData[] _weaponDatas;
    [Tooltip("‘•”õ‚µ‚Ä‚¢‚é•Ší")]
    private WeaponData _equipWeapon;
    private List<ISkillBase> _skillList = new();

    private PlayerStatus() 
    {
        Init();
    }
}

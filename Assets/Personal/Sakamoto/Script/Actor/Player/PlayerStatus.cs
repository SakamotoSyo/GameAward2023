using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerStatus
{
    public SkillBase NinjaThrowingKnives => _ninjaThrowingKnives;
    public SkillBase[] PlayerSkillList => _skillList;
    public WeaponData[] WeaponDatas => _weaponDatas;
    public PlayerEquipWeapon EquipWeapon => _equipWeapon;
    private WeaponData[] _weaponDatas = new WeaponData[4];
    [Tooltip("装備している武器")]
    private PlayerEquipWeapon _equipWeapon = new();
    [Tooltip("必殺技")]
    private SkillBase _ninjaThrowingKnives;
    private SkillBase[] _skillList = new SkillBase[2];

    private PlayerStatus() 
    {
        //TODO:試験的後で削除する
        for (int i = 0; i < _weaponDatas.Length; i++) 
        {
            _weaponDatas[i] = new(50,50,50,50, WeaponData.AttributeType.None, WeaponType.GreatSword); 
        }
        _equipWeapon.ChangeWeapon(_weaponDatas[0]);
    }

    public void ChangeWeponArray(WeaponData[] weaponDatas) 
    {
        _weaponDatas = weaponDatas;
    }

    /// <summary>
    /// パラメータの更新をして武器を入れ替える
    /// </summary>
    /// <param name="weaponData"></param>
    public void EquipWeponChange(WeaponData weaponData) 
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        _equipWeapon.ChangeWeapon(weaponData);
    }

    public float ConventionalAttack() 
    {
       return _equipWeapon.OffensivePower.Value;
    }

    public void SaveData() 
    {

    }
}

public class PlayerSaveData 
{
    public WeaponData EquipWeapon;
    public WeaponData[] WeaponArray;
    public SkillBase[] PlayerSkillList = new SkillBase[2];
    public SkillBase NinjaThrowingKnives;
}

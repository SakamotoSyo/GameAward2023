using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerStatus
{
    public ISkillBase[] PlayerSkillList => _skillList;
    public WeaponData[] WeponDatas => _weaponDatas;
    public PlayerEquipWeapon EquipWepon => _equipWeapon;
    private WeaponData[] _weaponDatas = new WeaponData[4];
    [Tooltip("装備している武器")]
    private PlayerEquipWeapon _equipWeapon = new();
    [Tooltip("必殺技")]
    private ISkillBase _ninjaThrowingKnives;
    private ISkillBase[] _skillList = new ISkillBase[2];

    private PlayerStatus() 
    {
        //試験的後で削除する
        for (int i = 0; i < _weaponDatas.Length; i++) 
        {
            _weaponDatas[i] = new(); 
            _weaponDatas[i].MaxDurable = 50;
            _weaponDatas[i].CurrentDurable = 50;
            _weaponDatas[i].OffensivePower = 50;
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
    public void EwuipWeponChange(WeaponData weaponData) 
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        _equipWeapon.ChangeWeapon(weaponData);
    }

    public void SaveData() 
    {

    }
}

public class PlayerSaveData 
{
    public WeaponData EquipWepon;
    public List<ISkillBase> PlayerSkillList = new();
}

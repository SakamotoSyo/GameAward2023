using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

public class PlayerStatus
{
    public WeaponData[] WeaponDatas => _weaponDatas;
    public PlayerEquipWeapon EquipWeapon => _equipWeapon;
    private WeaponData[] _weaponDatas = new WeaponData[4];
    [Tooltip("装備している武器")]
    private PlayerEquipWeapon _equipWeapon = new();
    private int _playerRankPoint = 0;

    private PlayerStatus()
    {
        //TODO:試験的後で削除する
        for (int i = 0; i < _weaponDatas.Length; i++)
        {
            _weaponDatas[i] = new(1000, 1000, 50, 1000, WeaponData.AttributeType.None, WeaponType.GreatSword);
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

    /// <summary>
    /// 使える武器を持っていた場合ランダムに入れ替える
    /// 入れ替えれなかった場合falseを返す
    /// </summary>
    public bool RandomEquipWeponChange()
    {
        _weaponDatas[_equipWeapon.WeaponNum].UpdateParam(_equipWeapon);
        var weaponArray = _weaponDatas.Where(x => 0 < x.CurrentDurable).ToArray();
        if (weaponArray.Length == 0)
        {
            Debug.Log("GameOver");
            return false;
        }
        else
        {
            _equipWeapon.ChangeWeapon(weaponArray[0]);
        }
        return true;
    }

    public float ConventionalAttack()
    {
        return _equipWeapon.OffensivePower.Value;
    }

    public void SaveStatus(PlayerSaveData saveData)
    {
        saveData.WeaponArray = _weaponDatas;
        saveData.PlayerRankPoint = _playerRankPoint;
    }

    public void LoadStatus(PlayerSaveData player)
    {
        _weaponDatas = player.WeaponArray;
        _equipWeapon.ChangeWeapon(player.WeaponArray[0]);
        _playerRankPoint = player.PlayerRankPoint;
    }
}

public class PlayerSaveData
{
    public WeaponData[] WeaponArray;
    public SkillBase[] PlayerSkillArray = new SkillBase[2];
    public SkillBase SpecialAttack;
    public int PlayerRankPoint;
}

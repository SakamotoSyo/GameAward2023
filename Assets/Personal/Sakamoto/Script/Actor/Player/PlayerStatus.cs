using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerStatus
{
    public List<ISkillBase> PlayerSkillList => _skillList;
    public WeaponData[] WeponDatas => _weaponDatas;
    public PlayerEquipWeapon EquipWepon => _equipWeapon;

    [Tooltip("次のレベルまでの経験値")]
    private int _maxExperienceAmount;
    [Tooltip("現在の経験値")]
    private int _currentExperienceAmount;
    [Tooltip("装備している武器")]
    private PlayerEquipWeapon _equipWeapon = new();
    private int _level;
    private int _skillPoint;
    private WeaponData[] _weaponDatas = new WeaponData[4];
    private List<ISkillBase> _skillList = new();

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
        _weaponDatas[_equipWeapon.WeaponNum].UpdategeParam(_equipWeapon);
        _equipWeapon.ChangeWeapon(weaponData);
    }

    /// <summary>
    /// 経験値の取得する処理
    /// </summary>
    public void ExperienceAcquisition(int experience) 
    {
        //レベルアップまでの経験値
        var SurplusExperience = _maxExperienceAmount - _currentExperienceAmount;

        if (SurplusExperience <= experience)
        {
            LevelUp();
            //レベルアップして余った経験値を追加で与える
            _currentExperienceAmount = experience - SurplusExperience;
        }
        else 
        {
            _currentExperienceAmount += experience;
        }
    }

    /// <summary>
    /// レベルアップの処理
    /// </summary>
    private void LevelUp() 
    {
        _level++;
        _maxExperienceAmount = DataBaseScript.GetMaxExperienceAmount(_level);
    }

    public void SaveData() 
    {

    }
}

public class PlayerSaveData 
{
    public int Level;
    public int MaxExperienceAmount;
    public int CurrentExperienceAmount;
    public int SkillPoint;
    public WeaponData EquipWepon;
    public List<ISkillBase> PlayerSkillList = new();
}

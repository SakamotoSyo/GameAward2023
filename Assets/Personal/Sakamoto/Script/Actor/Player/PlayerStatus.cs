using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using VContainer.Unity;

public class PlayerStatus : ActorStatusBase
{
    public List<ISkillBase> PlayerSkillList => _skillList;
    public WeaponData[] WeponDatas => _weaponDatas;
    public WeaponData EquipWepon => _equipWeapon;

    [Tooltip("次のレベルまでの経験値")]
    private int _maxExperienceAmount;
    [Tooltip("現在の経験値")]
    private int _currentExperienceAmount;
    [Tooltip("装備している武器")]
    private WeaponData _equipWeapon;
    private int _level;
    private int _skillPoint;
    private WeaponData[] _weaponDatas = new WeaponData[4];
    private List<ISkillBase> _skillList = new();

    private PlayerStatus() 
    {
        Init();
    }

    public void ChangeWeponArray(WeaponData[] weaponDatas) 
    {
        _weaponDatas = weaponDatas;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public Action GameOverAction;
    public PlayerStatus PlayerStatus => _playerStatus;

    [SerializeField, Tooltip("ダメージテキストのクラス")]
    private DamageTextController _damegeController;
    [SerializeField, Tooltip("ダメージテキストを生成する座標")]
    private Transform _damagePos;
    private PlayerStatus _playerStatus;
    [SerializeField] private PlayerAnimation _playerAnimation = new();
    private SkillDataManagement _skillDataManagement;

    private bool _isCounter = false;

    private void Start()
    {
        _skillDataManagement = GameObject.Find("SkillDataBase").GetComponent<SkillDataManagement>();
        _playerStatus.EquipWeapon.SetDebugSkill(_skillDataManagement.DebugSearchSkill());
        _playerStatus.EquipWeapon.Init(_skillDataManagement);
        _playerAnimation.Init(_playerStatus.EquipWeapon);
    }

    private void Update()
    {
       
    }

    /// <summary>
    /// ダメージを受ける流れ
    /// </summary>
    public void AddDamage(float damage) 
    {
        if (_playerStatus.EquipWeapon.IsEpicSkill1)
        {
            damage = 0;
        }

        var damageController = Instantiate(_damegeController,
          _damagePos.position,
           Quaternion.identity);
        damageController.TextInit((int)damage);

        if (_playerStatus.EquipWeapon.DownJudge(damage))
        {
            //アニメーションがあったらここでダメージを受ける処理を呼ぶ
            _playerStatus.EquipWeapon.AddDamage(damage);
        }
        else 
        {
            _playerStatus.EquipWeapon.AddDamage(damage);
            //武器が壊れたときに入れ替える処理
            if (!_playerStatus.RandomEquipWeponChange())
            {
                //GameOverの処理はここに
                Debug.Log("GameOver");
                GameOverAction?.Invoke();
            }
            else 
            {
                Debug.Log("入れ替えました");
            }
        }
    }

    /// <summary>
    /// スキルを使用する場合に呼ぶ関数
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    public bool UseSkillDamage(float damage)
    {
        if (_playerStatus.EquipWeapon.IsEpicSkill1)
        {
            damage = 1;
        }

        var damageController = Instantiate(_damegeController,
          _damagePos.position,
           Quaternion.identity);
        damageController.TextInit((int)damage);

        if (_playerStatus.EquipWeapon.DownJudge(damage))
        {
            //アニメーションがあったらここでダメージを受ける処理を呼ぶ
            _playerStatus.EquipWeapon.AddDamage(damage);
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 通常攻撃
    /// </summary>
    public float Attack(PlayerAttackType attackType) 
    {
        switch (attackType) 
        {
            case PlayerAttackType.ConventionalAttack:
                return _playerStatus.ConventionalAttack();
                break;
           // case PlayerAttackType.Skill1:
             //   return
        }
        return 0;
    }

    public void EquipWeaponChange(WeaponData weaponData, int arrayNum) 
    {
        _playerStatus.EquipWeponChange(weaponData, arrayNum);
        _playerAnimation.WeaponIdle();
    }

    public void SetPlayerStatus(PlayerStatus playerStatus) 
    {
        _playerStatus = playerStatus;
    }

    public void SavePlayerData() 
    {
        PlayerSaveData playerSaveData = new PlayerSaveData();
        _playerStatus.SaveStatus(playerSaveData);
        GameManager.SetPlayerData(playerSaveData);
        Debug.Log("Saveされました");
    }

    public void LoadPlayerData(PlayerSaveData playerSaveData) 
    {
        _playerStatus.LoadStatus(playerSaveData);
    }
}

public enum PlayerAttackType 
{
    ConventionalAttack,
    CounterAttack,
}

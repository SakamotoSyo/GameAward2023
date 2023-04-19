using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerStatus PlayerStatus => _playerStatus;

    private PlayerStatus _playerStatus;
    private PlayerAnimation _playerAnimation = new();

    private void Start()
    {
        
    }

    private void Update()
    {
       
    }

    /// <summary>
    /// ダメージを受ける流れ
    /// </summary>
    public void AddDamage(float damage) 
    {
        if (_playerStatus.EquipWeapon.DownJudge(damage))
        {
            //アニメーションがあったらここでダメージを受ける処理を呼ぶ
            _playerStatus.EquipWeapon.AddDamage(damage);
        }
        else 
        {
            //死んだときの処理を追加
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

    public void SetPlayerStatus(PlayerStatus playerStatus) 
    {
        _playerStatus = playerStatus;
    }
}

public enum PlayerAttackType 
{
    ConventionalAttack,
    NinjaThrowingKnives,
    Skill1,
    Skill2,
}

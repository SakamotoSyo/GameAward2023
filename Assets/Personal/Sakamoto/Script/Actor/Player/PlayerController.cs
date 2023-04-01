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

    /// <summary>
    /// ダメージを受ける流れ
    /// </summary>
    public void AddDamage(float damage) 
    {
        if (_playerStatus.DownJudge(damage))
        {
            //アニメーションがあったらここでダメージを受ける処理を呼ぶ
            _playerStatus.AddDamage(damage);
        }
        else 
        {
            //死んだときの処理を追加
        }
        
    }

    public void SetPlayerStatus(PlayerStatus playerStatus) 
    {
        _playerStatus = playerStatus;
    }
}

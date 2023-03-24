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
    /// ƒ_ƒ[ƒW‚ğó‚¯‚é—¬‚ê
    /// </summary>
    public void AddDamage(float damage) 
    {
        if (_playerStatus.DownJudge(damage))
        {
            _playerStatus.AddDamage(damage);
        }
        else 
        {
            //€‚ñ‚¾‚Æ‚«‚Ìˆ—‚ğ’Ç‰Á
        }
        
    }

    public void SetPlayerStatus(PlayerStatus playerStatus) 
    {
        _playerStatus = playerStatus;
    }
}

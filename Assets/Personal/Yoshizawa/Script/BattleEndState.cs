using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using State = StateMachine<StateController>.State;

// 日本語対応
public class BattleEndState : State
{
    protected override void OnEnter(State currentState)
    {
        Debug.Log($"「BattleEndState」に入りました。");
        WeaponData[] playerWeapons = Owner.PlayerController.PlayerStatus.WeaponDatas;

        // ゲームクリア判定
        if (Owner.BrokenWeaponsCount(playerWeapons) < playerWeapons.Length)
        {
            Owner.ResultUI.StartResultLottery();
            SceneManager.LoadScene(Owner.HomeScene);
        }
        // ゲームオーバー判定
        else
        {
            SceneManager.LoadScene(Owner.TitleScene);
        }
    }
}

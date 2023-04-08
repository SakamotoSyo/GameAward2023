using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class StateController : MonoBehaviour
{
    /// <summary>戦闘時、キャラクターの行動順序を格納するリスト</summary>
    private List<WeaponData> _orderOfAction = new List<WeaponData>();

    private StateMachine<StateController> _stateMachine = null;
    public StateMachine<StateController> StateMachine => _stateMachine;

    private enum TransitionCondition
    {
        Start2Player,
        Start2Enemy,
        Player2Enemy,
        Enemy2Enemy,
        Enemy2Player,
    }

    private void Start()
    {
        _stateMachine = new(this);
        _stateMachine.Start<BattleStartState>();
        _stateMachine.AddTransition<BattleStartState, PlayerStateAttack>((int)TransitionCondition.Start2Player);
        _stateMachine.AddTransition<BattleStartState, EnemyStateAttack>((int)TransitionCondition.Start2Enemy);
        _stateMachine.AddTransition<PlayerStateAttack, EnemyStateAttack>((int)TransitionCondition.Player2Enemy);
        _stateMachine.AddTransition<EnemyStateAttack, EnemyStateAttack>((int)TransitionCondition.Enemy2Enemy);
        _stateMachine.AddTransition<EnemyStateAttack, PlayerStateAttack>((int)TransitionCondition.Enemy2Player);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void CommandSelected()
    {

    }
}

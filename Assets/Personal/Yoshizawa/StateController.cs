using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<StateController>.State;

// 日本語対応
public class StateController : MonoBehaviour
{
    /// <summary>戦闘時、キャラクターの行動順序を格納するリスト</summary>
    private List<EnemyStatus> _orderOfAction = new List<EnemyStatus>();

    private StateMachine<StateController> _stateMachine = null;
    public StateMachine<StateController> StateMachine => _stateMachine;

    public enum TransitionCondition
    {
        Start2Player,
        Start2Enemy,
        Player2Enemy,
        Enemy2Enemy,
        Enemy2Player,
        Any2Start
    }

    private void OnEnable()
    {
        for (int i = 0; i < 3; i++)
        {
            _orderOfAction.Add(new EnemyStatus());
        }
    }

    private void Start()
    {
        _stateMachine = new(this);
        _stateMachine.Start<BattleStartState>();
        _stateMachine.AddTransition<BattleStartState, PlayerAttackState>((int)TransitionCondition.Start2Player);
        _stateMachine.AddTransition<BattleStartState, EnemyAttackState>((int)TransitionCondition.Start2Enemy);
        _stateMachine.AddTransition<PlayerAttackState, EnemyAttackState>((int)TransitionCondition.Player2Enemy);
        _stateMachine.AddTransition<EnemyAttackState, EnemyAttackState>((int)TransitionCondition.Enemy2Enemy);
        _stateMachine.AddTransition<EnemyAttackState, PlayerAttackState>((int)TransitionCondition.Enemy2Player);
        _stateMachine.AddAnyTranstion<BattleStartState>((int)TransitionCondition.Any2Start);
    }

    private void Update()
    {
        _stateMachine.Update();
    }

    public void CommandSelected()
    {

    }
}

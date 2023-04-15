using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class StateController : MonoBehaviour
{
    /// <summary>戦闘時、キャラクターの行動順序を格納するリスト</summary>
    private List<GameObject> _orderOfAction = new List<GameObject>();
    public List<GameObject> OrderOfAction => _orderOfAction;

    private StateMachine<StateController> _stateMachine = null;
    public StateMachine<StateController> StateMachine => _stateMachine;

    private EnemyController[] _enemyController = null;
    private PlayerController _playerController = null;
    public PlayerController PlayerController => _playerController;

    public enum TransitionCondition
    {
        Any2Start,
        Start2Player,
        Start2Enemy,
        Player2Enemy,
        Enemy2Enemy,
        Enemy2Player,
        Any2End,
    }

    private void OnEnable()
    {
        _enemyController = FindObjectsOfType<EnemyController>();
    }

    private void Start()
    {
        _stateMachine = new(this);
        _stateMachine.AddAnyTranstion<BattleStartState>((int)TransitionCondition.Any2Start);
        _stateMachine.AddTransition<BattleStartState, PlayerAttackState>((int)TransitionCondition.Start2Player);
        _stateMachine.AddTransition<BattleStartState, EnemyAttackState>((int)TransitionCondition.Start2Enemy);
        _stateMachine.AddTransition<PlayerAttackState, EnemyAttackState>((int)TransitionCondition.Player2Enemy);
        _stateMachine.AddTransition<EnemyAttackState, EnemyAttackState>((int)TransitionCondition.Enemy2Enemy);
        _stateMachine.AddTransition<EnemyAttackState, PlayerAttackState>((int)TransitionCondition.Enemy2Player);
        _stateMachine.AddAnyTranstion<BattleEndState>((int)TransitionCondition.Any2End);

        // 始めに遷移するステート
        _stateMachine.Start<BattleStartState>();
    }

    private void Update()
    {
        _orderOfAction.Sort((a, b) =>
        {
            float x = a.TryGetComponent(out EnemyController enemyA) ? enemyA.EnemyStatus.GetSpeed : 0f;
            float y = b.TryGetComponent(out EnemyController enemyB) ? enemyB.EnemyStatus.GetSpeed : 0f;

            if (x - y > 0)      return  1;
            else if (x - y < 0) return -1;
            else                return  0;
        });
        _stateMachine.Update();
    }

    public void CommandSelected()
    {
        if (BattleStartState.IsBattleStart)
        {
            if (_orderOfAction[0].TryGetComponent(out EnemyController enemyController))
            {
                _stateMachine.Dispatch((int)TransitionCondition.Start2Enemy);
            }
            else if (_orderOfAction[0].TryGetComponent(out PlayerController playerController))
            {
                _stateMachine.Dispatch((int)TransitionCondition.Start2Player);
            }
        }
    }
}

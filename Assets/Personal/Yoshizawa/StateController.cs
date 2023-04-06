using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class StateController : MonoBehaviour
{
    /// <summary>戦闘時、キャラクターの行動順序を格納するリスト</summary>
    private List<WeaponData> _orderOfAction = new List<WeaponData>();
    public List<WeaponData> OrderOfAction => _orderOfAction;

    private StateMachine<StateController> _stateMachine = null;
    public StateMachine<StateController> StateMachine => _stateMachine;

    private enum TransitionCondition
    {
        PlayerToEnemy,
        EnemyToEnemy,
        EnemyToPlayer,
    }

    private void Awake()
    {
        _stateMachine = new(this);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        _stateMachine.Update();
    }
}

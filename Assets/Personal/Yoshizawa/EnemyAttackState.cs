using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<StateController>.State;

// 日本語対応
public class EnemyAttackState : State
{
    protected override void OnEnter(State currentState)
    {
        Debug.Log($"「EnemyAttackState」に入りました。");
        //await Owner.CurrenyEnemy.Attack(Owner.PlayerController);
        Owner.NextStateTransition();
    }

    protected override void OnExit(State nextState)
    {
        Debug.Log($"{nextState.GetType()}に遷移するまで、3 2 1...");
    }
}

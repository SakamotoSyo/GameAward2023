using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<StateController>.State;

// 日本語対応
public class EnemyAttackState : State
{
    protected override void OnEnter(State currentState)
    {
        Debug.Log($"{currentState}に入りました。");
    }

    protected override void OnExit(State nextState)
    {
        Debug.Log($"{nextState}に遷移するまで、3 2 1...");
    }
}

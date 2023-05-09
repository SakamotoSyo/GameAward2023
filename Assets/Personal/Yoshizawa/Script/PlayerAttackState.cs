using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<StateController>.State;

// 日本語対応
public class PlayerAttackState : State
{
    protected override void OnEnter(State currentState)
    {
        Debug.Log($"「PlayerAttackState」に入りました。");
    }

    protected override void OnExit(State nextState)
    {
        Debug.Log($"「{nextState.GetType()}」に遷移するまで、3 2 1...");
    }
}

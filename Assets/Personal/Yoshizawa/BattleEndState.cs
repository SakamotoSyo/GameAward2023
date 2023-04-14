using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<StateController>.State;

// 日本語対応
public class BattleEndState : State
{
    protected override void OnEnter(State currentState)
    {
        Debug.Log($"{currentState}に入りました。");
    }
}

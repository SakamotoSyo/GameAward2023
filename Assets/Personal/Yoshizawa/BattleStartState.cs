using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<StateController>.State;

// 日本語対応
public class BattleStartState : State
{
    protected override void OnEnter(State currentState)
    {
        base.OnEnter(currentState);
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
    }
}

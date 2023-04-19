using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using State = StateMachine<BattleStateController>.State;

public class SelectNextActorTransitionState : State
{
    protected override async void OnEnter(State currentState)
    {
        Debug.Log("SelectState");
        Owner.NextActorStateTransition();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            
        }
    }

    protected override void OnExit(State nextState)
    {

    }
}

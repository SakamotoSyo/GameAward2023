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
      await Owner.NextActorStateTransition();
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnExit(State nextState)
    {

    }
}

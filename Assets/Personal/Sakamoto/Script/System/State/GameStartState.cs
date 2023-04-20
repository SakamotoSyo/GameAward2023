using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using State = StateMachine<BattleStateController>.State;

namespace SakamotoTest
{
    public class GameStartState : State
    {
        protected override async void OnEnter(State currentState)
        {
            Owner.ActionSequentialDetermining();
            await UniTask.Delay(1);
            StateMachine.Dispatch((int)BattleStateController.BattleEvent.StartToNextActorState);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected override void OnExit(State nextState)
        {

        }
    }
}



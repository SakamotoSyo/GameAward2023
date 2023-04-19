using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<BattleStateController>.State;

namespace SakamotoTest
{
    public class GameStartState : State
    {
        protected override void OnEnter(State currentState)
        {
            Owner.ActionSequentialDetermining();
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



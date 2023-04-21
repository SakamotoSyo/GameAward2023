using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<BattleStateController>.State;

namespace SakamotoTest
{
    public class BattleEndState : State
    {
        protected override void OnEnter(State currentState)
        {
            Owner.ResultUIScript.StartResultLottery();
        }

        protected override void OnExit(State nextState)
        {
            base.OnExit(nextState);
        }
    }
}

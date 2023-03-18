using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<BattleStateController>.State;

public class PlayerAttackState : State
{
    protected override void OnEnter(State currentState)
    {
        Owner.ComanndObj.SetActive(true);
    }

    protected override void OnExit(State nextState)
    {
        Owner.ComanndObj.SetActive(false);
    }
}

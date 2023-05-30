using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<BattleStateController>.State;

public class SBattleEndState : State
{
    protected override void OnEnter(State currentState)
    {
        Debug.Log("‚«‚½");
        //SoundManager.Instance.CriAtomBGMPlay("BGM_Result");
        Owner.SkillManagement.CallBattleFinish();
        Owner.ResultUIScript.StartResultLottery();
    }

    protected override void OnExit(State nextState)
    {
        base.OnExit(nextState);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State = StateMachine<StateController>.State;

// 日本語対応
public class BattleStartState : State
{
    public static bool IsBattleStart { get; private set; } = false;

    protected override void OnEnter(State currentState)
    {
        IsBattleStart = true;
        Debug.Log($"{currentState}に入りました。");
    }

    protected override void OnUpdate()
    {
        if (IsBattleStart)
        {
            
        }
    }

    protected override void OnExit(State nextState)
    {
        IsBattleStart = false;
        Debug.Log($"{nextState}に遷移するまで、3 2 1...");
    }
}

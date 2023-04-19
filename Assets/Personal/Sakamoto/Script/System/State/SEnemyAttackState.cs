using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using State = StateMachine<BattleStateController>.State;

public class SEnemyAttackState : State
{
    protected override async void OnEnter(State currentState)
    {
        Owner.EnemyController.Attack(Owner.PlayerController);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using State = StateMachine<BattleStateController>.State;

public class GameOverState : State
{
    protected override async void OnEnter(State currentState)
    {
        await TextActive();
    }

    public async UniTask TextActive() 
    {
        Owner.GameOverTextObj.SetActive(true);
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        SceneLoader.LoadScene("InitialPreparationScene");
    }
}

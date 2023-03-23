using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using VContainer;
using VContainer.Unity;

public class PlayerPresenter :IStartable, IDisposable
{
    private PlayerView _playerView;
    private PlayerStatus _playerStatus;
    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    PlayerPresenter(PlayerStatus playerStatus, PlayerView playerView, PlayerController playerController) 
    {
        _playerStatus = playerStatus;
        _playerView = playerView;
        playerController.SetPlayerStatus(playerStatus);
    }

    public void Start()
    {
        _playerStatus.MaxHp.Subscribe(_playerView.SetMaxHp).AddTo(_compositeDisposable);
        _playerStatus.CurrentHp.Subscribe(_playerView.SetCurrentHp).AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}

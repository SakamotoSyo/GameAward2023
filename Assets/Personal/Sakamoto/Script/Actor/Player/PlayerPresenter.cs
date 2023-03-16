using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using VContainer;

public class PlayerPresenter : IDisposable
{
    private PlayerView _playerView;
    private IPlayerStatus _playerStatus;
    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    PlayerPresenter(IPlayerStatus playerStatus, PlayerView playerView) 
    {
        _playerStatus = playerStatus;
        _playerView = playerView;
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}

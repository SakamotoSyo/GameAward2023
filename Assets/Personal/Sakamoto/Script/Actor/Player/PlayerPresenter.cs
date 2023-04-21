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
    /// <summary>çwì«ÇµÇƒÇ¢ÇÈÇ‡ÇÃÇÇ‹Ç∆ÇﬂÇƒDisposeÇµÇΩÇ¢Ç∆Ç´Ç…égÇ§ã@î\</summary>
    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    PlayerPresenter(PlayerStatus playerStatus, PlayerView playerView, PlayerController playerController) 
    {
        _playerStatus = playerStatus;
        _playerView = playerView;
        playerController.SetPlayerStatus(playerStatus);
        if (GameManager.PlayerSaveData != null) 
        {
            playerStatus.LoadStatus(GameManager.PlayerSaveData);
        }
    }

    public void Start()
    {
        _playerStatus.EquipWeapon.MaxDurable.Subscribe(_playerView.SetMaxHp).AddTo(_compositeDisposable);
        _playerStatus.EquipWeapon.CurrentDurable.Subscribe(_playerView.SetCurrentHp).AddTo(_compositeDisposable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}

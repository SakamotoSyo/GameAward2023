using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class PlayerLifeTimeScope : LifetimeScope
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerController _playerController;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IPlayerStatus, PlayerStatus>(Lifetime.Singleton);
        builder.RegisterComponent(_playerView);
        builder.RegisterComponent(_playerController);
        builder.RegisterEntryPoint<PlayerPresenter>();
    }

}

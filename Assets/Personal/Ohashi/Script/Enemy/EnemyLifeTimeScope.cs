using UnityEngine;
using VContainer;
using VContainer.Unity;

public class EnemyLifeTimeScope : LifetimeScope
{
    [SerializeField]
    private EnemyController _enemyController;

    [SerializeField]
    private EnemyView _enemyView;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<EnemyStatus>(Lifetime.Singleton);
        builder.RegisterComponent(_enemyController);
        builder.RegisterComponent(_enemyView);
        builder.RegisterEntryPoint<EnemyPresenter>();
    }
}

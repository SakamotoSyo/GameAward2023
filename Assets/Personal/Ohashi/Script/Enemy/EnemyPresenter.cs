using UnityEngine;
using UniRx;
using VContainer;
using VContainer.Unity;
using System;

public class EnemyPresenter : IStartable, IDisposable
{
    [SerializeField, Tooltip("モデル")]
    private EnemyStatus _enemyStatus;

    [SerializeField, Tooltip("エネミーのview")]
    private EnemyView _enemyView;

    private CompositeDisposable _compositeDisposable = new();

    [Inject]
    private EnemyPresenter(EnemyStatus enemyStatus, EnemyView enemyView, EnemyController enemyController)
    {
        _enemyStatus = enemyStatus;
        _enemyView = enemyView;
        
    }

    /// <summary>
    /// Halthの値を監視し、変更されたときSubscribeする
    /// </summary>
    private void EnemyHealthObserver()
    {
        
        //_enemyModel.EnemyHealth.Health
        //    .Subscribe(health => _enemyView.HealthText(health))
        //    .AddTo(_compositeDisposable);
    }

    public void Start()
    {
        EnemyHealthObserver();
    }

    public void Dispose()
    {
        //_compositeDisposable.Dispose();
    }
}

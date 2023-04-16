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
        enemyController.SetEnemyStatus(enemyStatus);
        
    }

    /// <summary>
    /// 武器の耐久値を監視し、変更されたときSubscribeする
    /// </summary>
    private void EnemyHealthObserver()
    {
        _enemyStatus.EquipWeapon.CurrentDurable
            .Subscribe(durable => _enemyView.HealthText(durable))
            .AddTo(_compositeDisposable);
    }

    public void Start()
    {
        EnemyHealthObserver();
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}

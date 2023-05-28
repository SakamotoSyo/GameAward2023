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
            .Subscribe(durable => 
            {
                if(durable <= 0)
                {
                    _enemyView.HealthText(0, _enemyStatus.EquipWeapon.MaxDurable);
                }
                else
                {
                    _enemyView.HealthText(durable, _enemyStatus.EquipWeapon.MaxDurable);
                }
            })
            .AddTo(_compositeDisposable);
    }

    public void Start()
    {
        EnemyHealthObserver();
        _enemyView.MaxHealthText(_enemyStatus.EquipWeapon.MaxDurable);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}

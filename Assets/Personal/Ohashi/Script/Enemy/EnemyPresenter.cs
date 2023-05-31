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
                if (durable <= 0)
                {
                    _enemyView.HealthText(0);
                }
                else
                {
                    _enemyView.HealthText(durable);
                }
            })
            .AddTo(_compositeDisposable);
    }

    private void EnemyMaxHealthObserver()
    {
        _enemyStatus.EquipWeapon.MaxDurable
            .Subscribe(max => _enemyView.MaxHealthText(max))
            .AddTo(_compositeDisposable);
    }

    private void EnemyWeaponTypeObserver()
    {
        _enemyStatus.EquipWeapon.WeaponTypeOb
             .Subscribe(_enemyView.ChangeWeaponIcon)
             .AddTo(_compositeDisposable);
    }

    public void Start()
    {
        EnemyHealthObserver();
        EnemyMaxHealthObserver();
        EnemyWeaponTypeObserver();
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}

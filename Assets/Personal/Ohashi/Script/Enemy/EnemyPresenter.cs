using UnityEngine;
using UniRx;
using VContainer;
using VContainer.Unity;
using System;

public class EnemyPresenter : IStartable, IDisposable
{
    [SerializeField, Tooltip("モデル")]
    private EnemyController _enemyModel;

    [SerializeField, Tooltip("エネミーのview")]
    private EnemyView _enemyView;

    /// <summary>
    /// Halthの値を監視し、変更されたときSubscribeする
    /// </summary>
    private void EnemyHealthObserver()
    {
        _enemyModel.EnemyHealth.Health
            .Subscribe(health => _enemyView.HealthText(health));
    }

    public void Start()
    {
        
    }

    public void Dispose()
    {
        
    }
}

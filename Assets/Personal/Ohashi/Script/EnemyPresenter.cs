using UnityEngine;
using UniRx;

public class EnemyPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("モデル")]
    private EnemyController _enemyModel;

    [SerializeField, Tooltip("エネミーのview")]
    private EnemyView _enemyView;

    private void Start()
    {
        EnemyHealthObserver();
    }

    /// <summary>
    /// Halthの値を監視し、変更されたときSubscribeする
    /// </summary>
    private void EnemyHealthObserver()
    {
        _enemyModel.Health
            .Subscribe(health => _enemyView.HealthText(health))
            .AddTo(this);
    }   

}

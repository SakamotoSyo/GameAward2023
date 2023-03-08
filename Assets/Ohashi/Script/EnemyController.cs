using UnityEngine;
using UniRx;

public class EnemyController : MonoBehaviour, IAddDamage
{
    [SerializeField, Tooltip("最大")]
    private float _maxHealth = 5f;

    private ReactiveProperty<float> _health = new();


    private void Start()
    {
        _health.Value = _maxHealth;
        HealthObserver();
    }

    /// <summary>
    /// healthの値を監視し、0以下になった時にSubscribeする
    /// </summary>
    private void HealthObserver()
    {
        _health
            .Where(health => health <= 0)
            .Subscribe(_ =>
            {
                Debug.Log("Enemyの体力が0になった");
                gameObject.SetActive(false);
            })
            .AddTo(this);
    }

    public void AddDamage(float damage)
    {
        _health.Value -= damage;
        Debug.Log($"Enemyは{damage}ダメージ受けた");
    }
}

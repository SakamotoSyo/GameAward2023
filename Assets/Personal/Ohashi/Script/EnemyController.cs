using UnityEngine;
using UniRx;

public class EnemyController : MonoBehaviour, IAddDamage
{
    [SerializeField, Tooltip("最大")]
    private float _maxHealth = 5f;

    public float MaxHealth => _maxHealth;

    private ReactiveProperty<float> _health = new();

    public IReadOnlyReactiveProperty<float> Health => _health;

    private void Start()
    {
        _health.Value = _maxHealth;
    }

    public void AddDamage(float damage)
    {
        _health.Value -= damage;
        Debug.Log($"Enemyは{damage}ダメージ受けた");
        if(_health.Value <= 0)
        {
            Debug.Log("Enemyの体力が0になった");
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using UniRx;

public class EnemyController : MonoBehaviour, IAddDamage
{
    [SerializeField, Tooltip("マックス時のHealth")]
    private float _maxHealth = 5f;

    [SerializeField, Tooltip("ダメージテキストのクラス")]
    private DamageTextController _damegeController;

    [SerializeField, Tooltip("ダメージテキストを生成する座標")]
    private Transform _damagePos;

    public float MaxHealth => _maxHealth;

    private ReactiveProperty<float> _health = new();

    public IReadOnlyReactiveProperty<float> Health => _health;

    private void Start()
    {
        _health.Value = _maxHealth;
    }

    public void AddDamage(int damage)
    {
        _health.Value -= damage;
        Debug.Log($"Enemyは{damage}ダメージ受けた");
        var damageController = Instantiate(_damegeController, 
            _damagePos.position,
            Quaternion.identity);
        damageController.TextInit(damage);
        if (_health.Value <= 0)
        {
            Debug.Log("Enemyの体力が0になった");
            gameObject.SetActive(false);
        }
    }
}

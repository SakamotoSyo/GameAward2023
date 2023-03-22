using UnityEngine;

public class EnemyController : MonoBehaviour, IAddDamage
{

    [SerializeField, Tooltip("ダメージテキストのクラス")]
    private DamageTextController _damegeController;

    [SerializeField, Tooltip("ダメージテキストを生成する座標")]
    private Transform _damagePos;

    [SerializeField]
    private EnemyHealth _enemyHealth;

    public EnemyHealth EnemyHealth => _enemyHealth;

    private SpriteRenderer _renderer;

    private Animator _animator;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _enemyHealth.Init(_renderer, _animator, gameObject);
    }

    public void AddDamage(int damage)
    {
        _enemyHealth.DamageAnimation();
        var damageController = Instantiate(_damegeController,
            _damagePos.position,
            Quaternion.identity);
        damageController.TextInit(damage);
        _enemyHealth.Damage(damage);
    }
}

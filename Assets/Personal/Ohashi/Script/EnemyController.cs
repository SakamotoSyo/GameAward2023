using UnityEngine;

public class EnemyController : MonoBehaviour, IAddDamage
{

    [SerializeField, Tooltip("ダメージテキストのクラス")]
    private DamageTextController _damegeController;

    [SerializeField, Tooltip("ダメージテキストを生成する座標")]
    private Transform _damagePos;

    [SerializeField]
    private SpriteRenderer _renderer;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private EnemyHealth _enemyHealth;

    [SerializeField]
    private EnemyAttack _enemyAttack;

    public EnemyHealth EnemyHealth => _enemyHealth;

    private void Start()
    {
        _enemyHealth.Init(_renderer, _animator, gameObject);
        _enemyAttack.Init(_animator);
    }

    public void Attack(PlayerController playerController)
    {
        _enemyAttack.Attack(playerController);
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

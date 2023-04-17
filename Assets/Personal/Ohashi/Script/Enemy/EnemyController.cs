using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour, IAddDamage
{

    [SerializeField, Tooltip("ダメージテキストのクラス")]
    private DamageTextController _damegeController;

    [SerializeField, Tooltip("ダメージテキストを生成する座標")]
    private Transform _damagePos;

    private SpriteRenderer _renderer;

    private Animator _animator;

    private EnemyAttack _enemyAttack = new();

    private EnemyAnimation _enemyAnimation = new();

    private EnemyStatus _enemyStatus;

    public EnemyStatus EnemyStatus => _enemyStatus;

    private void Start()
    {
        _enemyAttack.Init(_enemyStatus.EquipWeapon.OffensivePower);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 攻撃のメソッドを呼ぶ
    /// </summary>
    public void Attack(PlayerController playerController)
    {
        _enemyAttack.Attack(playerController);
    }

    public void AddDamage(int damage)
    {
        var damageController = Instantiate(_damegeController,
            _damagePos.position,
            Quaternion.identity);
        damageController.TextInit(damage);
    }

    /// <summary>
    /// EnemyStatusを参照する
    /// </summary>
    public void SetEnemyStatus(EnemyStatus enemyStatus)
    {
        _enemyStatus = enemyStatus;
    }

    /// <summary>
    /// EnemyDateから参照してくる
    /// </summary>
    public void SetEnemyData(EnemyData enemyDate)
    {
        _enemyStatus.SetWeaponDates(enemyDate);
        _animator = enemyDate.EnemyAnim;
        _renderer.sprite = enemyDate.EnemySprite;
    }
}

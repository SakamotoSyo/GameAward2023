using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyController : MonoBehaviour, IAddDamage
{

    [SerializeField, Tooltip("ダメージテキストのクラス")]
    private DamageTextController _damegeController;

    [SerializeField, Tooltip("ダメージテキストを生成する座標")]
    private Transform _damagePos;

    [SerializeField]
    private SpriteRenderer _renderer;

    private Animator _animator;

    private EnemyAttack _enemyAttack = new();

    private EnemyAnimation _enemyAnimation = new();

    private EnemyStatus _enemyStatus;

    public EnemyStatus EnemyStatus => _enemyStatus;

    private EnemyColor _enemyColor;

    public EnemyColor EnemyColor => _enemyColor;

    private void Start()
    {
        _enemyAttack.Init(_enemyStatus.EquipWeapon);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 攻撃のメソッドを呼ぶ
    /// </summary>
    public async UniTask Attack()
    {
        if(!_enemyStatus.IsStan)
        {
            _enemyAttack.SelectAttack();
            await UniTask.Delay(1);
        }
    }

    public void AddDamage(float damage, float criticalRate)
    {
        var damageController = Instantiate(_damegeController,
            _damagePos.position,
            Quaternion.identity);
        damageController.TextInit((int)damage, _enemyStatus.EquipWeapon.AddDamage((int)damage, criticalRate));

        if(_enemyStatus.EquipWeapon.IsWeaponBreak())
        {
            if(_enemyStatus.IsWeaponsAllBrek())
            {
                Debug.Log("敵の武器が全部壊れた");
            }

            _enemyStatus.EquipChangeWeapon();
        }
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
    public void SetEnemyData(EnemyData enemyData)
    {
        _enemyStatus.SetWeaponDates(enemyData);
        _animator = enemyData.EnemyAnim;
        _renderer.sprite = enemyData.EnemySprite;
        _enemyColor = enemyData.EnemyColor;
    }
}

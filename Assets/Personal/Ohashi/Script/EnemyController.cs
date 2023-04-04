using System.Collections;
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

    [SerializeField]
    private Transform[] _damageObjects;

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
        int r = Random.Range(0, _damageObjects.Length);

        _enemyHealth.DamageAnimation();
        var damageController = Instantiate(_damegeController,
            _damageObjects[r].position,
            Quaternion.identity);
        damageController.TextInit(damage);
        _enemyHealth.Damage(damage);
    }

    public void TwinSwordAdDamage(int damage)
    {
        StartCoroutine(DamageTextInterval(damage));
        _enemyHealth.DamageAnimation();
        _enemyHealth.Damage(damage);
    }

    IEnumerator DamageTextInterval(int damage)
    {

        for (int i = 0; i < _damageObjects.Length; i++)
        {
            var damageController = Instantiate(_damegeController,
                _damageObjects[i].position,
                Quaternion.identity);
            damageController.TextInit(damage / _damageObjects.Length);
            yield return new WaitForSeconds(0.05f);
        }
    }
}

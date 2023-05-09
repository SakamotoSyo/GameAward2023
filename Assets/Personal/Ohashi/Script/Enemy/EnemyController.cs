﻿using System.Collections;
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

    private void Start()
    {
        _enemyAttack.Init(_enemyStatus.EquipWeapon.CurrentOffensivePower, _animator);
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// 攻撃のメソッドを呼ぶ
    /// </summary>
    public async UniTask Attack(PlayerController playerController)
    {
        if(!_enemyStatus.IsStan)
        {
            _enemyAttack.SelectAttack(playerController);
            await UniTask.Delay(1);
        }
    }

    public void AddDamage(float damage)
    {
        var damageController = Instantiate(_damegeController,
            _damagePos.position,
            Quaternion.identity);
        damageController.TextInit((int)damage);
        _enemyStatus.EquipWeapon.AddDamage((int)damage);

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
    public void SetEnemyData(EnemyData enemyDate)
    {
        _enemyStatus.SetWeaponDates(enemyDate);
        _animator = enemyDate.EnemyAnim;
        _renderer.sprite = enemyDate.EnemySprite;
    }
}

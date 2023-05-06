using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KasokugiriSkill : SkillBase
{
    [Tooltip("攻撃間の待機時間")] [SerializeField] private int _attackWaitTime;

    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    private EnemyStatus _enemyStatus;
    private ActorAttackType _actor;
    private bool _isUse = false;

    public KasokugiriSkill()
    {
        SkillName = "加速斬り";
        Damage = 20;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon,
        ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _actor = actorType;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override async void SkillEffect()
    {
        _isUse = true;
        int num = 0;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
                float weight = _playerStatus.EquipWeapon.WeaponWeight.Value;
                switch (Mathf.FloorToInt(weight / 10))
                {
                    case 1:
                        // 重さが20以下の場合の処理
                        num = 8;
                        for (int i = 0; i < num; i++)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                cancellationToken: this.GetCancellationTokenOnDestroy());
                            _enemyStatus.EquipWeapon.AddDamage(_playerStatus.EquipWeapon.OffensivePower.Value);
                        }

                        break;
                    case 2:
                        // 重さが21~30以下の場合の処理
                        num = 7;
                        for (int i = 0; i < num; i++)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                cancellationToken: this.GetCancellationTokenOnDestroy());
                            _enemyStatus.EquipWeapon.AddDamage(_playerStatus.EquipWeapon.OffensivePower.Value);
                        }

                        break;
                    case 3:
                        // 重さが31~40以下の場合の処理
                        num = 6;
                        for (int i = 0; i < num; i++)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                cancellationToken: this.GetCancellationTokenOnDestroy());
                            _enemyStatus.EquipWeapon.AddDamage(_playerStatus.EquipWeapon.OffensivePower.Value);
                        }

                        break;
                    default:
                        // 重さが40より大きい場合の処理
                        num = 5;
                        for (int i = 0; i < num; i++)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                cancellationToken: this.GetCancellationTokenOnDestroy());
                            _enemyStatus.EquipWeapon.AddDamage(_playerStatus.EquipWeapon.OffensivePower.Value);
                        }

                        break;
                }
            }
                break;
            case ActorAttackType.Enemy:
            {
                _enemyStatus.EquipWeapon.OffensivePower += Damage;
                float weight = _enemyStatus.EquipWeapon.WeaponWeight;
                switch (Mathf.FloorToInt(weight / 10))
                {
                    case 1:
                        // 重さが20以下の場合の処理
                        num = 8;
                        for (int i = 0; i < num; i++)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                cancellationToken: this.GetCancellationTokenOnDestroy());
                            _playerStatus.EquipWeapon.AddDamage(_enemyStatus.EquipWeapon.OffensivePower);
                        }

                        break;
                    case 2:
                        // 重さが21~30以下の場合の処理
                        num = 7;
                        for (int i = 0; i < num; i++)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                cancellationToken: this.GetCancellationTokenOnDestroy());
                            _playerStatus.EquipWeapon.AddDamage(_enemyStatus.EquipWeapon.OffensivePower);
                        }

                        break;
                    case 3:
                        // 重さが31~40以下の場合の処理
                        num = 6;
                        for (int i = 0; i < num; i++)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                cancellationToken: this.GetCancellationTokenOnDestroy());
                            _playerStatus.EquipWeapon.AddDamage(_enemyStatus.EquipWeapon.OffensivePower);
                        }

                        break;
                    default:
                        // 重さが40より大きい場合の処理
                        num = 5;
                        for (int i = 0; i < num; i++)
                        {
                            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                cancellationToken: this.GetCancellationTokenOnDestroy());
                            _playerStatus.EquipWeapon.AddDamage(_enemyStatus.EquipWeapon.OffensivePower);
                        }

                        break;
                }
            }
                break;
        }
    }


    public override void TurnEnd()
    {
        if (!_isUse)
        {
            return;
        }

        _isUse = false;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
            }
                break;
            case ActorAttackType.Enemy:
            {
                _enemyStatus.EquipWeapon.OffensivePower -= Damage;
            }
                break;
        }
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}
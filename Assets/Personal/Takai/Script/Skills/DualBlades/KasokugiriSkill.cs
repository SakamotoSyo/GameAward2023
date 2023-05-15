using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KasokugiriSkill : SkillBase
{
    [Tooltip("攻撃間の待機時間")][SerializeField] private int _attackWaitTime;
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;

    public KasokugiriSkill()
    {
        SkillName = "加速斬り";
        Damage = 20;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
        FlavorText = "重さが軽いほど連撃数が増える(上限4)";
    }

    private void Start()
    {
        _anim = GetComponent<PlayableDirector>();
    }

    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _actor = actorType;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        int num = 0;

        switch (_actor)
        {
            case ActorAttackType.Player:
                {
                    float weight = _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value;
                    switch (Mathf.FloorToInt(weight / 10))
                    {
                        case 1:
                            // 重さが20以下の場合の処理
                            num = 8;
                            for (int i = 0; i < num; i++)
                            {
                                AddDamage();
                            }

                            break;
                        case 2:
                            // 重さが21~30以下の場合の処理
                            num = 7;
                            for (int i = 0; i < num; i++)
                            {
                                AddDamage();
                            }

                            break;
                        case 3:
                            // 重さが31~40以下の場合の処理
                            num = 6;
                            for (int i = 0; i < num; i++)
                            {
                                AddDamage();
                            }

                            break;
                        default:
                            // 重さが40より大きい場合の処理
                            num = 5;
                            for (int i = 0; i < num; i++)
                            {
                                AddDamage();
                            }

                            break;
                    }
                }
                break;
            case ActorAttackType.Enemy:
                {
                    float weight = _enemyStatus.EnemyStatus.EquipWeapon.WeaponWeight;
                    switch (Mathf.FloorToInt(weight / 10))
                    {
                        case 1:
                            // 重さが20以下の場合の処理
                            num = 8;
                            for (int i = 0; i < num; i++)
                            {
                                AddDamage();
                            }

                            break;
                        case 2:
                            // 重さが21~30以下の場合の処理
                            num = 7;
                            for (int i = 0; i < num; i++)
                            {
                                AddDamage();
                            }

                            break;
                        case 3:
                            // 重さが31~40以下の場合の処理
                            num = 6;
                            for (int i = 0; i < num; i++)
                            {
                                AddDamage();
                            }

                            break;
                        default:
                            // 重さが40より大きい場合の処理
                            num = 5;
                            for (int i = 0; i < num; i++)
                            {
                                AddDamage();
                            }

                            break;
                    }
                }
                break;
        }
    }

    private async void AddDamage()
    {
        switch (_actor)
        {
            case ActorAttackType.Player:
                await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                   cancellationToken: this.GetCancellationTokenOnDestroy());
                _enemyStatus.AddDamage(
                    _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
                break;
            case ActorAttackType.Enemy:
                await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime),
                                    cancellationToken: this.GetCancellationTokenOnDestroy());
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.OffensivePower + Damage);
                break;
        }
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
    }
}
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class KasokugiriSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private PlayableDirector _enemyAnim;
    [SerializeField] private GameObject _enemyObj;

    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;

    public KasokugiriSkill()
    {
        SkillName = "加速斬り";
        Damage = 20;
        RequiredPoint = 30;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
        FlavorText = "重さが軽いほど連撃数が増える(上限4)";
    }
    
    public override bool IsUseCheck(ActorGenerator actor)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _actor = actorType;
        if (_actor == ActorAttackType.Player)
        {
            _playerObj.SetActive(true);
            _playerStatus.gameObject.SetActive(false);
            _playerAnim.Play();
            var dura = _playerAnim.duration * 0.99f;
            await UniTask.WaitUntil(() => _playerAnim.time >= dura,
                cancellationToken: this.GetCancellationTokenOnDestroy());
            SkillEffect();
            _playerAnim.Stop();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _playerStatus.gameObject.SetActive(true);
            Debug.Log("Anim End");
            _playerObj.SetActive(false);
        }
        else if (_actor == ActorAttackType.Enemy)
        {
            _enemyObj.SetActive(true);
            _enemyStatus.gameObject.SetActive(false);
            _enemyAnim.Play();
            var dura = _enemyAnim.duration * 0.99f;
            await UniTask.WaitUntil(() => _enemyAnim.time >= dura,
                cancellationToken: this.GetCancellationTokenOnDestroy());
            SkillEffect();
            _enemyAnim.Stop();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _enemyStatus.gameObject.SetActive(true);
            Debug.Log("Anim End");
            _enemyObj.SetActive(false);
        }
    }

    protected override void SkillEffect()
    {
        int num = 0;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                float weight = _playerStatus.PlayerStatus.EquipWeapon.GetWeightPram();

                if (weight >= 41)
                {
                    // 41以上の処理
                    num = 5;
                    for (int i = 0; i < num; i++)
                    {
                        AddDamage();
                    }
                }
                else if (weight >= 31)
                {
                    // 31~40の処理
                    num = 6;
                    for (int i = 0; i < num; i++)
                    {
                        AddDamage();
                    }
                }
                else if (weight >= 21)
                {
                    // 21~30の処理
                    num = 7;
                    for (int i = 0; i < num; i++)
                    {
                        AddDamage();
                    }
                }
                else
                {
                    // 20以下の処理
                    num = 8;
                    for (int i = 0; i < num; i++)
                    {
                        AddDamage();
                    }
                }

                break;
            }
            case ActorAttackType.Enemy:
            {
                float weight = _enemyStatus.EnemyStatus.EquipWeapon.WeaponWeight;
                if (weight >= 41)
                {
                    // 41以上の処理
                    num = 5;
                    for (int i = 0; i < num; i++)
                    {
                        AddDamage();
                    }
                }
                else if (weight >= 31)
                {
                    // 31~40の処理
                    num = 6;
                    for (int i = 0; i < num; i++)
                    {
                        AddDamage();
                    }
                }
                else if (weight >= 21)
                {
                    // 21~30の処理
                    num = 7;
                    for (int i = 0; i < num; i++)
                    {
                        AddDamage();
                    }
                }
                else
                {
                    // 20以下の処理
                    num = 8;
                    for (int i = 0; i < num; i++)
                    {
                        AddDamage();
                    }
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
                await UniTask.Delay(TimeSpan.FromSeconds(0),
                    cancellationToken: this.GetCancellationTokenOnDestroy());
                _enemyStatus.AddDamage(
                    _playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() + Damage);
                break;
            case ActorAttackType.Enemy:
                await UniTask.Delay(TimeSpan.FromSeconds(0),
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
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class TenkuugiriSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    private const float WeaponWeight = 100;
    private const float AddDamageValue = 0.2f;
    private bool _isUse = false;

    public TenkuugiriSkill()
    {
        SkillName = "天空斬り";
        Damage = 180;
        RequiredPoint = 5;
        Weapon = (WeaponType)0;
        Type = (SkillType)1;
        FlavorText = "重さが100以上の時のときこの技の攻撃力が20%上がる ※使用後武器破壊";
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
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        SkillEffect();
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        _isUse = true;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                var dmg = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value;

                if (_playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value >= WeaponWeight)
                {
                    _enemyStatus.AddDamage(
                      dmg + Damage + (dmg * AddDamageValue ));
                }
                else
                {
                    _enemyStatus.AddDamage(dmg + Damage);
                }
            }
                break;
            case ActorAttackType.Enemy:
            {
                var dmg = _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower;

                if (_enemyStatus.EnemyStatus.EquipWeapon.WeaponWeight >= WeaponWeight)
                {
                    _playerStatus.AddDamage(dmg + Damage +(dmg * AddDamageValue));
                }
                else
                {
                    _playerStatus.AddDamage(dmg + Damage);
                }
            }
                break;
            default:
                break;
        }
    }

    public override bool TurnEnd()
    {
        if (!_isUse)
        {
            return false;
        }

        _isUse = false;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                _playerStatus.PlayerStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
                break;
            case ActorAttackType.Enemy:
            {
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
                break;
        }

        return false;
    }

    public override void BattleFinish()
    {
    }
}
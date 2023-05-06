using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class TenkuugiriSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    private EnemyStatus _enemyStatus;
    private ActorAttackType _actor;
    private const float WeaponWeight = 100;
    private const float AddDamageValue = 0.2f;
    private float _attackValue = 0;
    private bool _isUse = false;

    public TenkuugiriSkill()
    {
        SkillName = "天空斬り";
        Damage = 180;
        Weapon = (WeaponType)0;
        Type = (SkillType)1;
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

    protected override void SkillEffect()
    {
        _isUse = true;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                var dmg = _playerStatus.EquipWeapon.OffensivePower.Value;

                if (_playerStatus.EquipWeapon.WeaponWeight.Value >= WeaponWeight)
                {
                    _attackValue += dmg * AddDamageValue + Damage;
                    _playerStatus.EquipWeapon.OffensivePower.Value += dmg * AddDamageValue + Damage;
                }
                else
                {
                    _attackValue += Damage;
                    _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
                }
            }
                break;
            case ActorAttackType.Enemy:
            {
                var dmg = _enemyStatus.EquipWeapon.OffensivePower;

                if (_enemyStatus.EquipWeapon.WeaponWeight >= WeaponWeight)
                {
                    _attackValue += dmg * AddDamageValue + Damage;
                    _enemyStatus.EquipWeapon.OffensivePower += dmg * AddDamageValue + Damage;
                }
                else
                {
                    _attackValue += Damage;
                    _enemyStatus.EquipWeapon.OffensivePower += Damage;
                }
            }
                break;
            default:
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
                _playerStatus.EquipWeapon.OffensivePower.Value -= _attackValue;
                _attackValue = 0;
                _playerStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
                break;
            case ActorAttackType.Enemy:
            {
                _enemyStatus.EquipWeapon.OffensivePower-= _attackValue;
                _attackValue = 0;
                _enemyStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
                break;
        }

        
    }

    public override void BattleFinish()
    {
        _isUse = false;
        _attackValue = 0;
    }
}
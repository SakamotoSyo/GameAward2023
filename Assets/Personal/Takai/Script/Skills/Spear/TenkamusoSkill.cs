
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class TenkamusoSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    private int _count;
    private bool _isUse = false;

    public TenkamusoSkill()
    {
        SkillName = "天下無双";
        Damage = 120;
        Weapon = (WeaponType)3;
        Type = (SkillType)1;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _actor = actorType;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                var hp = _playerStatus.PlayerStatus.EquipWeapon.CurrentDurable.Value * 0.3f;
                if (_playerStatus.PlayerStatus.EquipWeapon.CurrentDurable.Value <= hp)
                {
                    _isUse = true;
                    _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage + (_count * 10);
                }
            }
                break;
            case ActorAttackType.Enemy:
            {
                var hp = _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value * 0.3f;
                if (_enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value <= hp)
                {
                    _isUse = true;
                    _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower += Damage + (_count * 10);
                }
            }
                break;
        }
    }

    public override void TurnEnd()
    {
        _count++;

        if (!_isUse)
        {
            return;
        }

        _isUse = false;

        switch (_actor)
        {
            case ActorAttackType.Player:
                _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= Damage + ((_count - 1) * 10);
                break;
            case ActorAttackType.Enemy:
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower -= Damage + ((_count - 1) * 10);
                break;
        }
        
    }

    public override void BattleFinish()
    {
        _count = 0;
        _isUse = false;
    }
}
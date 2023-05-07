using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class PowerBlowSkill : SkillBase
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
    private bool _isUse;

    public PowerBlowSkill()
    {
        SkillName = "パワーブロウ";
        Damage = 60;
        Weapon = (WeaponType)2;
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

    protected override void SkillEffect()
    {
        _isUse = true;
        
        switch (_actor)
        {
            case ActorAttackType.Player:
                _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
                break;
            case ActorAttackType.Enemy:
                _enemyStatus.EquipWeapon.CurrentOffensivePower += Damage;
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
                _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
                break;
            case ActorAttackType.Enemy:
                _enemyStatus.EquipWeapon.CurrentOffensivePower -= Damage;
                break;
        }
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}
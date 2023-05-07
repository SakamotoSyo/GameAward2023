using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class RenzokugiriSkill : SkillBase
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
    private bool _isUse = false;

    public RenzokugiriSkill()
    {
        SkillName = "連続斬り";
        Damage = 30;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
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
        // スキルの効果処理を実装する
        switch (_actor)
        {
            case ActorAttackType.Player:
                _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage;
                break;
            case ActorAttackType.Enemy:
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower += Damage;
                break;
        }
    }

    public override void TurnEnd()
    {
        if (!_isUse)
        {
            return;
        }

        _isUse = true;
        // スキルの効果処理を実装する
        switch (_actor)
        {
            case ActorAttackType.Player:
                _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= Damage;
                break;
            case ActorAttackType.Enemy:
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower -= Damage;
                break;
        }
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class ZenryokuuchiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;

    public ZenryokuuchiSkill()
    {
        SkillName = "全力打ち";
        Damage = 60;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "効果なし";
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
        switch (_actor)
        {
            case ActorAttackType.Player:
                _playerStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
                break;
            case ActorAttackType.Enemy:
                _enemyStatus.AddDamage((int)_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower + Damage);
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
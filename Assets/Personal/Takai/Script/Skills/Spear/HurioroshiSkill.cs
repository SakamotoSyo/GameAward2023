using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using DG.Tweening;

public class HurioroshiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    private bool _isUse = false;

    public HurioroshiSkill()
    {
        SkillName = "振り下ろし";
        Damage = 60;
        Weapon = (WeaponType)3;
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
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        _isUse = true;

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
    
    public override void TurnEnd()
    {
        if (!_isUse)
        {
            return;
        }
        
        _isUse = false;
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}
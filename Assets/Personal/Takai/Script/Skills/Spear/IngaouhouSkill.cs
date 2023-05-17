using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IngaouhouSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    
    public IngaouhouSkill()
    {
        SkillName = "因果応報";
        Damage = 70;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "発動したターンに攻撃を受けるとダメージを30%軽減し、反撃する。";
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
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {

    }

    public override async UniTask InEffectSkill(ActorAttackType attackType) 
    {
        if (attackType == ActorAttackType.Player)
        {
            _anim.Play();
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
            await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
        else 
        {
            _anim.Play();
            // _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
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
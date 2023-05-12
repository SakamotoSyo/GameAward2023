using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IkishochinSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    float _subtractValue = 0.2f;
    public IkishochinSkill()
    {
        SkillName = "意気消沈";
        Damage = 60;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "敵の攻撃力20%を下げる";
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
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
        _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower -= _enemyStatus.EnemyStatus.EquipWeapon.OffensivePower + _subtractValue;
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
        
    }
}
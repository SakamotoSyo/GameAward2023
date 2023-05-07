using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IkishochinSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
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
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage;
        _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower -= _enemyStatus.EnemyStatus.EquipWeapon.OffensivePower + _subtractValue;
    }

    public override void TurnEnd()
    {
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= Damage;
    }

    public override void BattleFinish()
    {
    }
}
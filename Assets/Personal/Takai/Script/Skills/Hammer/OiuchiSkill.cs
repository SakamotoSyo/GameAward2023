using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class OiuchiSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    private EnemyStatus _enemyStatus;
    bool _isDebuff = false;

    public OiuchiSkill()
    {
        SkillName = "追い打ち";
        Damage = 45;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon, ActorAttackType actorType)
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
        _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
        if (true) //敵にデバフがついているか検知
        {
            _isDebuff = true;
            _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
        }
    }

    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
        if (_isDebuff) //敵にデバフがついているか検知
        {
            _isDebuff = false;
            _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
        }
    }

    public override void BattleFinish()
    {
        _isDebuff = false;
    }
}
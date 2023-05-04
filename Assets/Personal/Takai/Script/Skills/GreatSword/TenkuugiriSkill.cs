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
    private const float WeaponWeight = 0;
    private const float AddDamageValue = 0.2f;
    private float _attackValue = 0;

    public TenkuugiriSkill()
    {
        SkillName = "天空斬り";
        Damage = 180;
        Weapon = (WeaponType)0;
        Type = (SkillType)1;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        var dmg = _playerStatus.EquipWeapon.OffensivePower.Value;
        // スキルの効果処理を実装する
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

    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= _attackValue;
        _attackValue = 0;
        _playerStatus.EquipWeapon.CurrentDurable.Value = 0;
    }

    public override void BattleFinish()
    {
        _attackValue = 0;
    }
}
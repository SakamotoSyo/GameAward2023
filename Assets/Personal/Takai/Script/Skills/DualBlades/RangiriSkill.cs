using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Experimental.GlobalIllumination;

public class RangiriSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    const float AddDamageValue = 0.05f;
    const int Turn = 3;
    float _attackValue = 0;
    int _count = 0;

    public RangiriSkill()
    {
        SkillName = "乱切り";
        Damage = 60;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
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
        // スキルの効果処理を実装する
        float dmg = _playerStatus.EquipWeapon.OffensivePower.Value;
        if (_count >= Turn)
        {
            _count++;
            _attackValue += (dmg * (AddDamageValue * _count)) + Damage;
            _playerStatus.EquipWeapon.OffensivePower.Value += (dmg * (AddDamageValue * _count));
        }
    }

    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= _attackValue;
    }


    public override void BattleFinish()
    {
        _count = 0;
        _attackValue = 0;
    }
}
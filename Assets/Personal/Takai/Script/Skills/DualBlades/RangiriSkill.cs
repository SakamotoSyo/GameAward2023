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
    private PlayerController _playerStatus;
    const float AddDamageValue = 0.05f;
    const int Turn = 3;
    float _attackValue = 0;
    int _count = 0;
    bool _isUse = false;

    public RangiriSkill()
    {
        SkillName = "乱切り";
        Damage = 60;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
        FlavorText = "2ターンの間攻撃力が5%上昇(重複あり→5%,10%,15%)";
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
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
        _isUse = true;

        float dmg = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value;
        if (_count >= Turn)
        {
            _count++;
            _attackValue += (dmg * (AddDamageValue * _count)) + Damage;
            _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += (dmg * (AddDamageValue * _count));
        }
    }

    public override void TurnEnd()
    {
        if (!_isUse)
        {
            return;
        }

        _isUse = false;
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= _attackValue;
    }


    public override void BattleFinish()
    {
        _isUse = false;
        _count = 0;
        _attackValue = 0;
    }
}
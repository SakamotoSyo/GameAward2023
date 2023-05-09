using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Experimental.GlobalIllumination;

public class RangiriSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    const float AddDamageValue = 0.05f;
    const int Turn = 3;
    int _count = 0;

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
        float dmg = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value;
        if (_count >= Turn)
        {
            _count++;
            _playerStatus.AddDamage(dmg * (AddDamageValue * _count));
        }
    }

    public override bool TurnEnd()
    {
        return false;
    }


    public override void BattleFinish()
    {
        _count = 0;
    }
}
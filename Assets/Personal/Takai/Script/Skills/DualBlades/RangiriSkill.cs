using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Experimental.GlobalIllumination;

public class RangiriSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    const float AddDamageValue = 0.05f;
    const int Turn = 3;
    private int _count = 0;
    private int _turnCount;

    public RangiriSkill()
    {
        SkillName = "乱切り";
        Damage = 60;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
        FlavorText = "2ターンの間攻撃力が5%上昇(重複あり→5%,10%,15%)";
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
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        float dmg = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value;

        _count = (_count <= Turn) ? _count++ : _count;

        //_enemyStatus.AddDamage(_count);
        _enemyStatus.AddDamage(((AddDamageValue * _count) * dmg) + Damage + dmg);
    }

    public override bool TurnEnd()
    {
        if (++_turnCount >= 3)
        {
            _count--;
            _turnCount = 0;
        }

        return true;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}

using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class HaisuiNoJinSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;

    public HaisuiNoJinSkill()
    {
        SkillName = "背水の刃";
        Damage = 0;
        Weapon = (WeaponType)4;
        Type = (SkillType)2;
        FlavorText = "耐久値が永久に5になる 受けるダメージが0になるが、あらゆる攻撃の実行時に耐久値をｰ1する";
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
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {

    }
}
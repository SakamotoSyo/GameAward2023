using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class TemplateSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _status;

    public TemplateSkill()
    {
        SkillName = "テンプレート";
        Damage = 0;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        _anim = GetComponent<PlayableDirector>();
    }

    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _status = player;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
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
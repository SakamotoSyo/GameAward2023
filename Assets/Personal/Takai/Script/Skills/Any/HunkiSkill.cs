
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class HunkiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;

    public HunkiSkill()
    {
        SkillName = "奮起";
        Damage = 0;
        Weapon = (WeaponType)4;
        Type = (SkillType)2;
        FlavorText = "重さが相手の重さより小さかったら(相手より速かったら)、再行動できる。(1ターンに一度)。 ただし、2回目は通常攻撃のみ";
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
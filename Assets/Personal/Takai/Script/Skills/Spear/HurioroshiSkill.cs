
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class HurioroshiSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;

    public HurioroshiSkill()
    {
        SkillName = "振り下ろし";
        Damage = 60;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus status)
    {
        Debug.Log("Use Skill");
        _anim = GetComponent<PlayableDirector>();
        SkillEffect(status);
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
        Debug.Log("Anim End");
    }

    protected override void SkillEffect(PlayerStatus status)
    {
        // スキルの効果処理を実装する
    }
}
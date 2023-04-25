using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KiriageSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }

    private PlayableDirector _anim;
    private const int AddDamageValue = 5;
    private int _count = 0;
    private int _attackValue;

    public KiriageSkill()
    {
        SkillName = "斬り上げ";
        Damage = 70;
        Weapon = (WeaponType)0;
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
        if (_count <= 2)
        {
            _count++;
            status.EquipWeapon.OffensivePower.Value += AddDamageValue * _count;
            _attackValue += _attackValue;
        }
        else
        {
            _attackValue += _attackValue;
        }
    }

    public override void BattleFinish()
    {
        _count = 0;
        _attackValue = 0;
    }
}
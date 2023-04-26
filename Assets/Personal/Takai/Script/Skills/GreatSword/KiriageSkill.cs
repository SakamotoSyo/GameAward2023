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

    private PlayerStatus _status;
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

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
    {
        Debug.Log("Use Skill");
        _status = player;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
        Debug.Log("Anim End"); 
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        if (_count <= 2)
        {
            _count++;
            _status.EquipWeapon.OffensivePower.Value += (AddDamageValue * _count) + Damage;
            _attackValue += AddDamageValue * _count;
        }
        else
        {
            _status.EquipWeapon.OffensivePower.Value += Damage;
        }
    }

    public override void TurnEnd()
    {
        _status.EquipWeapon.OffensivePower.Value -= Damage;
    }


    public override void BattleFinish()
    {
        _status.EquipWeapon.OffensivePower.Value -= _attackValue;
        _count = 0;
        _attackValue = 0;
    }
}
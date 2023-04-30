using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class SeishintouitsuSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    const float AddValue = 0.2f;
    private int _count;
    private float _value;

    public SeishintouitsuSkill()
    {
        SkillName = "精神統一";
        Damage = 0;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _count += 4;
        _value += _playerStatus.EquipWeapon.CriticalRate.Value * (1 + AddValue);
        _playerStatus.EquipWeapon.CriticalRate.Value += _playerStatus.EquipWeapon.CriticalRate.Value * (1 + AddValue);
        // 会心時のダメージが20%上昇
    }

    public override void TurnEnd()
    {
        if (_count <= 0)
        {
            _playerStatus.EquipWeapon.CriticalRate.Value -= _value;
            _value = 0;
        }
        else
        {
            _count--;
        }
    }

    public override void BattleFinish()
    {
        _value = 0;
        _count = 0;
    }
}
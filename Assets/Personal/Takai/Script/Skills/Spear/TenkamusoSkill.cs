
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class TenkamusoSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    private int _count;

    public TenkamusoSkill()
    {
        SkillName = "天下無双";
        Damage = 120;
        Weapon = (WeaponType)3;
        Type = (SkillType)1;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
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
        var hp = _playerStatus.EquipWeapon.CurrentDurable.Value * 0.3f;
        if (_playerStatus.EquipWeapon.CurrentDurable.Value <= hp)
        {

            _playerStatus.EquipWeapon.OffensivePower.Value += Damage + (_count * 10);

        }
    }

    public override void TurnEnd()
    {
        _count++;
        _playerStatus.EquipWeapon.OffensivePower.Value -= Damage + (_count * 10);
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
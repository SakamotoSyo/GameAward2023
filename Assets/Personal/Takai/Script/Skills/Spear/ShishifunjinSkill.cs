using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class ShishifunjinSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    private int _count;

    public ShishifunjinSkill()
    {
        SkillName = "獅子奮迅";
        Damage = 20;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon, ActorAttackType actorType)
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
        _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
        //経過ターンが多いほど攻撃回数アップ（上限 7回） 1 + 2×(ターン数-1) 
        if (_count >= 7)
        {
            int num = 1 + 2 * (7 - 1);
        }
        else
        {
            int num = 1 + 2 * (_count - 1);
        }
    }

    public override void TurnEnd()
    {
        _count++;
        _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
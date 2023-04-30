
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class HauchiSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    private EnemyStatus _enemyStatus;
    const float _subtractAttackValue = 0.2f;
    public HauchiSkill()
    {
        SkillName = "刃打ち";
        Damage = 50;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
        _enemyStatus.EquipWeapon.OffensivePower -= _enemyStatus.EquipWeapon.OffensivePower + _subtractAttackValue;
        _enemyStatus.EquipWeapon.CriticalRate -= _enemyStatus.EquipWeapon.CriticalRate * _subtractAttackValue;
    }
    
    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
    }

    public override void BattleFinish()
    {
        
    }
}
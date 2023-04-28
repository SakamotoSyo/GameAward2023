
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KasokuSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    const float AddSpeedValue = 0.05f;
    const int Turn = 3;
    float _speedValue = 0;
    int _count = 0;

    public KasokuSkill()
    {
        SkillName = "加速";
        Damage = 0;
        Weapon = (WeaponType)1;
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
        //float spd = _playerStatus.EquipWeapon.
        //if (_count >= Turn)
        //{
        //    _count++;
        //    _attackValue += (dmg * (AddDamageValue * _count)) + Damage;
        //    _playerStatus.EquipWeapon.OffensivePower.Value += (dmg * (AddDamageValue * _count));
        //}
    }
    
    public override void TurnEnd()
    {
        
    }

    public override void BattleFinish()
    {
        
    }
}
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class ShinsokuranbuSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    bool _isAttack = false;

    public ShinsokuranbuSkill()
    {
        SkillName = "神速乱舞";
        Damage = 150;
        Weapon = (WeaponType)1;
        Type = (SkillType)1;
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
        float weight = _playerStatus.EquipWeapon.WeaponWeight.Value;


        if (weight <= 30) //素早さをに応じて発動できるか検知
        {
            _isAttack = true;
            _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
        }
    }

    public override void TurnEnd()
    {
        if (_isAttack)
        {
            _isAttack = false;
            _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
        }

    }

    public override void BattleFinish()
    {
        _isAttack = false;  
    }
}
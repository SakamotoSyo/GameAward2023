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
    private EnemyStatus _enemyStatus;
    private ActorAttackType _actor;
    bool _isUse = false;

    public ShinsokuranbuSkill()
    {
        SkillName = "神速乱舞";
        Damage = 150;
        Weapon = (WeaponType)1;
        Type = (SkillType)1;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon,
        ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _actor = actorType;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        float weight = 0;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                weight = _playerStatus.EquipWeapon.WeaponWeight.Value;

                if (weight <= 30) //素早さをに応じて発動できるか検知
                {
                    _isUse = true;
                    _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
                }
            }
                break;
            case ActorAttackType.Enemy:
            {
                weight = _enemyStatus.EquipWeapon.WeaponWeight;

                if (weight <= 30) //素早さをに応じて発動できるか検知
                {
                    _isUse = true;
                    _enemyStatus.EquipWeapon.CurrentOffensivePower += Damage;
                }
            }
                break;
        }
        // スキルの効果処理を実装する
    }

    public override void TurnEnd()
    {
        if (!_isUse)
        {
            return;
        }

        _isUse = false;

        switch (_actor)
        {
            case ActorAttackType.Player:
                _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
                break;
            case ActorAttackType.Enemy:
                _enemyStatus.EquipWeapon.CurrentOffensivePower -= Damage;
                break;
        }
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}
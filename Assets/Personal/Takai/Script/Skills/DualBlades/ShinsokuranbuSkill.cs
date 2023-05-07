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
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    bool _isUse = false;

    public ShinsokuranbuSkill()
    {
        SkillName = "神速乱舞";
        Damage = 150;
        Weapon = (WeaponType)1;
        Type = (SkillType)1;
        FlavorText = "重さが30以下のとき発動可能　※使用後元のステータスに戻る";
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
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
                weight = _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value;

                if (weight <= 30) //素早さをに応じて発動できるか検知
                {
                    _isUse = true;
                    _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage;
                }
            }
                break;
            case ActorAttackType.Enemy:
            {
                weight = _enemyStatus.EnemyStatus.EquipWeapon.WeaponWeight;

                if (weight <= 30) //素早さをに応じて発動できるか検知
                {
                    _isUse = true;
                    _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower += Damage;
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
                _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= Damage;
                break;
            case ActorAttackType.Enemy:
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower -= Damage;
                break;
        }
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}
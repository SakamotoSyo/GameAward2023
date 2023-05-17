using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class HauchiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    const float _subtractValue = 0.4f;

    public HauchiSkill()
    {
        SkillName = "刃打ち";
        Damage = 50;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "敵の攻撃力と会心率が20%下がる";
    }

    private void Start()
    {
        _anim = GetComponent<PlayableDirector>();
    }


    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _actor = actorType;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        FluctuationStatusClass fluctuation;
        switch (_actor)
        {
            case ActorAttackType.Player:
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
                fluctuation = new FluctuationStatusClass(
                    -_enemyStatus.EnemyStatus.EquipWeapon.OffensivePower * _subtractValue, 0,
                    -_enemyStatus.EnemyStatus.EquipWeapon.CriticalRate * _subtractValue, 0, 0);
            _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
                break;
            case ActorAttackType.Enemy:
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.OffensivePower + Damage);
                fluctuation = new FluctuationStatusClass(
                    - _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value * _subtractValue, 0,
                    -_playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value * _subtractValue, 0, 0);
                _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
                break;
        }
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
    }
}
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class GekiretsuKudakiuchiSkill : SkillBase
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
    const float _subtractValue = 0.4f;
    private bool _isUse;

    public GekiretsuKudakiuchiSkill()
    {
        SkillName = "激烈・砕き打ち";
        Damage = 170;
        Weapon = (WeaponType)2;
        Type = (SkillType)1;
        FlavorText = "敵の攻撃、会心率、素早さを40%下げる　※使用した武器は壊れる";
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
        _isUse = true;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                // スキルの効果処理を実装する
                _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage;

                // 防御、素早さを40%下げる。
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower -=
                    _enemyStatus.EnemyStatus.EquipWeapon.OffensivePower * _subtractValue;
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentCriticalRate -=
                    _enemyStatus.EnemyStatus.EquipWeapon.CriticalRate * _subtractValue;
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentWeaponWeight -=
                    _enemyStatus.EnemyStatus.EquipWeapon.WeaponWeight * _subtractValue;

                _playerStatus.PlayerStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
                break;
            case ActorAttackType.Enemy:
            {
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower += Damage;

                // 防御、素早さを40%下げる。
                _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -=
                    _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value * _subtractValue;
                _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value -=
                    _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value * _subtractValue;
                _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value -=
                    _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value * _subtractValue;

                _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
                break;
        }
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
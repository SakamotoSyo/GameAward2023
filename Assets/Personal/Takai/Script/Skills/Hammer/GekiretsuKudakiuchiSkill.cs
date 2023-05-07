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
    private PlayerStatus _playerStatus;
    private EnemyStatus _enemyStatus;
    private ActorAttackType _actor;
    const float _subtractValue = 0.4f;
    private bool _isUse;

    public GekiretsuKudakiuchiSkill()
    {
        SkillName = "激烈・砕き打ち";
        Damage = 170;
        Weapon = (WeaponType)2;
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
        _isUse = true;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                // スキルの効果処理を実装する
                _playerStatus.EquipWeapon.OffensivePower.Value += Damage;

                // 防御、素早さを40%下げる。
                _enemyStatus.EquipWeapon.CurrentOffensivePower -=
                    _enemyStatus.EquipWeapon.OffensivePower * _subtractValue;
                _enemyStatus.EquipWeapon.CurrentCriticalRate -=
                    _enemyStatus.EquipWeapon.CriticalRate * _subtractValue;
                _enemyStatus.EquipWeapon.CurrentWeaponWeight -=
                    _enemyStatus.EquipWeapon.WeaponWeight * _subtractValue;

                _playerStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
                break;
            case ActorAttackType.Enemy:
            {
                _enemyStatus.EquipWeapon.CurrentOffensivePower += Damage;

                // 防御、素早さを40%下げる。
                _playerStatus.EquipWeapon.OffensivePower.Value -=
                    _playerStatus.EquipWeapon.OffensivePower.Value * _subtractValue;
                _playerStatus.EquipWeapon.CriticalRate.Value -=
                    _playerStatus.EquipWeapon.CriticalRate.Value * _subtractValue;
                _playerStatus.EquipWeapon.WeaponWeight.Value -=
                    _playerStatus.EquipWeapon.WeaponWeight.Value * _subtractValue;

                _enemyStatus.EquipWeapon.CurrentDurable.Value = 0;
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
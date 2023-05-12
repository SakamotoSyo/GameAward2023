using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class GekiretsuKudakiuchiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    const float _subtractValue = 0.4f;

    public GekiretsuKudakiuchiSkill()
    {
        SkillName = "激烈・砕き打ち";
        Damage = 170;
        Weapon = (WeaponType)2;
        Type = (SkillType)1;
        FlavorText = "敵の攻撃、会心率、素早さを40%下げる　※使用した武器は壊れる";
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
        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                // スキルの効果処理を実装する
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);

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
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower + Damage);

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

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
        
    }
}
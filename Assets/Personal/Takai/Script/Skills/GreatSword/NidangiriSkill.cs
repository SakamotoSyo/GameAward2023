using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class NidangiriSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;

 
    public NidangiriSkill()
    {
        SkillName = "2段斬り";
        Damage = 80;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "重さが大きいほど2撃目のダメージが大きくなる(上限4)";
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
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        float dmg = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value;
        float weight = _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value;
        
        _playerStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);

        switch (weight / 10)
        {
            case 6:
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + 20);
                break;
            case 5:
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + 15);
                break;
            case 4:
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + 10);
                break;
            case 3:
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + 5);
                break;
            default:
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
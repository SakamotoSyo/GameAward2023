using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class TenkamusoSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    private int _count;

    public TenkamusoSkill()
    {
        SkillName = "天下無双";
        Damage = 120;
        RequiredPoint = 5;
        Weapon = (WeaponType)3;
        Type = (SkillType)1;
        FlavorText = "経過ターンが多いほど威力上昇　※HP30％以下で発動可能";
    }
    
    public override bool IsUseCheck(PlayerController player)
    {
        var hp = player.PlayerStatus.EquipWeapon.CurrentDurable.Value * 0.3f;
        return (player.PlayerStatus.EquipWeapon.CurrentDurable.Value >= hp) ? true : false;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _actor = actorType;
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        SkillEffect();
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage +
                                       (_count * 10));
            }
                break;
            case ActorAttackType.Enemy:
            {
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower + Damage +
                                        (_count * 10));
            }
                break;
        }
    }

    public override bool TurnEnd()
    {
        _count++;

        return true;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
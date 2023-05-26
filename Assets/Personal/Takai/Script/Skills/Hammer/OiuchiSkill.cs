using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class OiuchiSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;

    public OiuchiSkill()
    {
        SkillName = "追い打ち";
        Damage = 1.45f;
        RequiredPoint = 20;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "敵に弱体効果がついていると威力が上がる。";
    }

    public override bool IsUseCheck(ActorGenerator actor)
    {
        _playerStatus = actor.PlayerController;
        _enemyStatus = actor.EnemyController;
        
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        SkillEffect();
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        if (_enemyStatus.EnemyStatus.IsDebuff()) //敵にデバフがついているか検知
        {
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * 2.2f,
                _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
        }
        else
        {
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * Damage,
            _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
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
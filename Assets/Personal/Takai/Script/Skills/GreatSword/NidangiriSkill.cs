using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class NidangiriSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;

 
    public NidangiriSkill()
    {
        SkillName = "2段斬り";
        Damage = 1.2f;
        RequiredPoint = 30;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "素早さが低いほど2撃目のダメージが大きくなる。(上限4)";
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
        float dmg = _playerStatus.PlayerStatus.EquipWeapon.GetPowerPram();
        float weight = _playerStatus.PlayerStatus.EquipWeapon.GetWeightPram() / 10;
        
        _enemyStatus.AddDamage(dmg * Damage,_playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());

        if (weight >= 5)
        {
            _enemyStatus.AddDamage(dmg * (Damage + 0.1f + 0.05f),_playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
        }
        else if(weight >= 4)
        {
            _enemyStatus.AddDamage(dmg * (Damage + 0.1f + 0.1f)+ Damage,_playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
        }
        else if (weight >= 3)
        {
            _enemyStatus.AddDamage(dmg * (Damage + 0.1f + 0.15f)+ Damage,_playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
        }
        else 
        {
            _enemyStatus.AddDamage(dmg * (Damage + 0.1f + 0.2f)+ Damage,_playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
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
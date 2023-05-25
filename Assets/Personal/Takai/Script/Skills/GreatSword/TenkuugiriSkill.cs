using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class TenkuugiriSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    private const float WeaponWeight = 100;
    private const float AddDamageValue = 0.2f;
    private bool _isUse = false;

    public TenkuugiriSkill()
    {
        SkillName = "天空斬り";
        Damage = 180;
        RequiredPoint = 0;
        Weapon = (WeaponType)0;
        Type = (SkillType)1;
        FlavorText = "重さが100以上の時のときこの技の攻撃力が20%上がる ※使用後武器破壊";
    }

    public override bool IsUseCheck(ActorGenerator actor)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _actor = actorType;

        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _playerAnim.Play();
        var dura = _playerAnim.duration * 0.99f;
        await UniTask.WaitUntil(() => _playerAnim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        SkillEffect();
        _playerAnim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        _isUse = true;


        var dmg = _playerStatus.PlayerStatus.EquipWeapon.GetPowerPram();

        if (_playerStatus.PlayerStatus.EquipWeapon.GetWeightPram() >= WeaponWeight)
        {
            _enemyStatus.AddDamage(
                dmg + Damage + (dmg * AddDamageValue),
                _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
        }
        else
        {
            _enemyStatus.AddDamage(dmg + Damage, _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
        }
    }

    public override bool TurnEnd()
    {
        if (!_isUse)
        {
            return false;
        }
        _isUse = false;
        _playerStatus.PlayerStatus.EquipWeapon.CurrentDurable.Value = 0;
        
        return false;
    }

    public override void BattleFinish()
    {
    }
}
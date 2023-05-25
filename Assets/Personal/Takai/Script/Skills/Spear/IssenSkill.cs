using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using DG.Tweening;
using UnityEngine.Serialization;

public class IssenSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private PlayableDirector _enemyAnim;
    [SerializeField] private GameObject _enemyObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    
    public IssenSkill()
    {
        SkillName = "一閃";
        Damage = 1;
        RequiredPoint = 10;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "効果なし";
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
        if (_actor == ActorAttackType.Player)
        {
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
        else if (_actor == ActorAttackType.Enemy)
        {
            _enemyObj.SetActive(true);
            _enemyStatus.gameObject.SetActive(false);
            _enemyAnim.Play();
            var dura = _enemyAnim.duration * 0.99f;
            await UniTask.WaitUntil(() => _enemyAnim.time >= dura,
                cancellationToken: this.GetCancellationTokenOnDestroy());
            SkillEffect();
            _enemyAnim.Stop();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _enemyStatus.gameObject.SetActive(true);
            Debug.Log("Anim End");
            _enemyObj.SetActive(false);
        }
    }

    protected override void SkillEffect()
    {
        switch (_actor)
        {
            case ActorAttackType.Player:
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * Damage,_playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
                break;
            case ActorAttackType.Enemy:
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower * Damage,_enemyStatus.EnemyStatus.EquipWeapon.CriticalRate);
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
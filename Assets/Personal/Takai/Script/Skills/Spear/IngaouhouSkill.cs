using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using System;
using UnityEngine.Serialization;

public class IngaouhouSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private PlayableDirector _enemyAnim;
    [SerializeField] private GameObject _enemyObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;

    public IngaouhouSkill()
    {
        SkillName = "因果応報";
        Damage = 70;
        RequiredPoint = 5;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "発動したターンに攻撃を受けるとダメージを30%軽減し、反撃する。";
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
    }

    public override async UniTask<bool> InEffectSkill(ActorAttackType attackType)
    {
        if (attackType == ActorAttackType.Player)
        {
            _playerObj.SetActive(true);
            _playerAnim.Play();
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
            await UniTask.WaitUntil(() => _playerAnim.time >= _playerAnim.duration - 0.1,
                cancellationToken: this.GetCancellationTokenOnDestroy());
            await UniTask.WaitUntil(() => _playerAnim.time >= _playerAnim.duration - 0.1, cancellationToken: this.GetCancellationTokenOnDestroy());
            _playerObj.SetActive(false);
        }
        else
        {
            _playerAnim.Play();
            // _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
        }

        return true;
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
    }
}
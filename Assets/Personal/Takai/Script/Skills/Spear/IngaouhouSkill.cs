using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using System;
using UnityEngine.Serialization;

public class IngaouhouSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim1;
    [SerializeField] private PlayableDirector _playerAnim2;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private bool _isUse = false;
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

    public override bool IsUseCheck(ActorGenerator actor)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _isUse = false;

        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _playerAnim2.Play();
        var dura = _playerAnim2.duration * 0.99f;
        await UniTask.WaitUntil(() => _playerAnim2.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        SkillEffect();
        _playerAnim2.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
    }

    public override async UniTask<bool> InEffectSkill(ActorAttackType attackType)
    {
        if (!_isUse)
        {
            Debug.Log("因果味方");
            _isUse = true;
            _playerStatus.gameObject.SetActive(false);
            _playerObj.SetActive(true);
            _playerAnim1.Play();
            var dura = _playerAnim1.duration * 0.99f;
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * (Damage * 0.1f));
            await UniTask.WaitUntil(() => _playerAnim1.time >= dura,
                cancellationToken: this.GetCancellationTokenOnDestroy());
            _playerAnim1.Stop();
            await UniTask.Delay(TimeSpan.FromSeconds(0.5));
            _playerObj.SetActive(false);
            _playerStatus.gameObject.SetActive(true);
            return true;
        }

        return false;
    }

    public override bool TurnEnd()
    {
        if (_isUse)
        {
            return true;
        }

        return false;
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}
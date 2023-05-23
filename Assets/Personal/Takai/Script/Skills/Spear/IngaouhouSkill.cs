using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using System;

public class IngaouhouSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim1;
    [SerializeField] private PlayableDirector _playerAnim2;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private PlayableDirector _enemyAnim;
    [SerializeField] private GameObject _enemyObj;
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

    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _isUse = false;
        if (_actor == ActorAttackType.Player)
        {
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
        if (!_isUse)
        {
<<<<<<< HEAD
            _playerObj.SetActive(true);
            _playerAnim1.Play();
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
            await UniTask.WaitUntil(() => _playerAnim1.time >= _playerAnim1.duration - 0.1,
                cancellationToken: this.GetCancellationTokenOnDestroy());
            await UniTask.WaitUntil(() => _playerAnim1.time >= _playerAnim1.duration - 0.1, cancellationToken: this.GetCancellationTokenOnDestroy());
            _playerObj.SetActive(false);
        }
        else
        {
            _playerAnim1.Play();
            // _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
=======
            if (attackType == ActorAttackType.Player)
            {
                Debug.Log("因果味方");
                _isUse = true;
                _playerObj.SetActive(true);
                _playerAnim.Play();
                var dura = _playerAnim.duration * 0.99f;
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * (Damage * 0.1f));
                await UniTask.WaitUntil(() => _playerAnim.time >= dura,
                    cancellationToken: this.GetCancellationTokenOnDestroy());
                _playerAnim.Stop();
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                _playerObj.SetActive(false);
            }
            else
            {
                Debug.Log("因果敵");
                _isUse = true;
                _playerAnim.Play();
                _playerAnim.Stop();
                // _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.GetPowerPram() * (Damage * 0.01f));
            }
            return true;
>>>>>>> d57a78aa8c972fb18544023017ed86dea65af1cb
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
    }
}
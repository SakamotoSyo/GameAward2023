using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using System;
using UnityEngine.Serialization;

public class ShishifunjinSkill : SkillBase
{
    [Tooltip("攻撃間の待機時間")] [SerializeField] private int _attackWaitTime;
    [SerializeField] private PlayableDirector _playerAnim;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private PlayableDirector _enemyAnim;
    [SerializeField] private GameObject _enemyObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    private int _count;

    public ShishifunjinSkill()
    {
        SkillName = "獅子奮迅";
        Damage = 0.6f;
        RequiredPoint = 35;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "経過ターンが多いほど攻撃回数アップ（上限 7回）";
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
        _actor = actorType;

        switch (_actor)
        {
            case ActorAttackType.Player:
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
                break;
            case ActorAttackType.Enemy:
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
                break;
        }
    }

    protected override async void SkillEffect()
    {
        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                var token = this.GetCancellationTokenOnDestroy();
                // スキルの効果処理を実装する
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * Damage,
                    _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
                //経過ターンが多いほど攻撃回数アップ（上限 7回） 1 + 2×(ターン数-1) 
                int num = 1 + 2 * (_count - 1);
                if (7 < num) num = 7;

                for (int i = 0; i < num; i++)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime), cancellationToken: token);
                    _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * Damage,
                        _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
                }
            }
                break;
            case ActorAttackType.Enemy:
            {
                var token = this.GetCancellationTokenOnDestroy();
                // スキルの効果処理を実装する
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.OffensivePower * Damage,
                    _enemyStatus.EnemyStatus.EquipWeapon.CriticalRate);
                //経過ターンが多いほど攻撃回数アップ（上限 7回） 1 + 2×(ターン数-1) 
                int num = 1 + 2 * (_count - 1);
                if (7 < num) num = 7;

                for (int i = 0; i < num; i++)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime), cancellationToken: token);
                    _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.OffensivePower * Damage,
                        _enemyStatus.EnemyStatus.EquipWeapon.CriticalRate);
                }
            }
                break;
        }
    }

    public override bool TurnEnd()
    {
        _count++;

        return false;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
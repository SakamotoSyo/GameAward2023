using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class ShinsokuranbuSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private PlayableDirector _enemyAnim;
    [SerializeField] private GameObject _enemyObj;

    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;

    public ShinsokuranbuSkill()
    {
        SkillName = "神速乱舞";
        Damage = 150;
        RequiredPoint = 50;
        Weapon = (WeaponType)1;
        Type = (SkillType)1;
        FlavorText = "重さが30以下のとき発動可能　※使用後元のステータスに戻る";
    }

    public override bool IsUseCheck(ActorGenerator actor)
    {
        if (actor.PlayerController)
        {
            float weight = actor.PlayerController.PlayerStatus.EquipWeapon.WeaponWeight.Value;
            return (weight >= 100) ? true : false;
        }
        else if (actor.EnemyController)
        {
            float weight = actor.EnemyController.EnemyStatus.EquipWeapon.WeaponWeight;
            return (weight >= 100) ? true : false;
        }

        return false;
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
        float weight = 0;

        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                weight = _playerStatus.PlayerStatus.EquipWeapon.GetWeightPram();
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() + Damage);
            }
                break;
            case ActorAttackType.Enemy:
            {
                weight = _enemyStatus.EnemyStatus.EquipWeapon.WeaponWeight;
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower + Damage);
            }
                break;
        }
    }

    public override bool TurnEnd()
    {
        //ステータスが元に戻る処理

        return false;
    }

    public override void BattleFinish()
    {
    }
}
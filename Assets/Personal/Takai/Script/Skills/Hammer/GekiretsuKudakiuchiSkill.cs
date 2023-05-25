using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class GekiretsuKudakiuchiSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private PlayableDirector _enemyAnim;
    [SerializeField] private GameObject _enemyObj;

    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    const float _subtractValue = 0.5f;

    public GekiretsuKudakiuchiSkill()
    {
        SkillName = "激烈・砕き打ち";
        Damage = 17;
        RequiredPoint = 0;
        Weapon = (WeaponType)2;
        Type = (SkillType)1;
        FlavorText = "敵の攻撃、会心率、素早さを大幅に下げる。使用した武器は壊れる。（50%・永続・重複なし）";
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
        FluctuationStatusClass fluctuation;
        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                // スキルの効果処理を実装する
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * Damage,
                    _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());

                fluctuation =
                    new FluctuationStatusClass(-_enemyStatus.EnemyStatus.EquipWeapon.OffensivePower * _subtractValue,
                        -_enemyStatus.EnemyStatus.EquipWeapon.WeaponWeight * _subtractValue,
                        -_enemyStatus.EnemyStatus.EquipWeapon.CriticalRate * _subtractValue, 0, 0);
                _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
                _playerStatus.PlayerStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
                break;
            case ActorAttackType.Enemy:
            {
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower * Damage,
                    _enemyStatus.EnemyStatus.EquipWeapon.CriticalRate);

                fluctuation =
                    new FluctuationStatusClass(-_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * _subtractValue,
                        -_playerStatus.PlayerStatus.EquipWeapon.GetWeightPram() * _subtractValue,
                        -_playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram() * _subtractValue * _subtractValue, 0,
                        0);
                _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
                _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value = 0;
            }
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
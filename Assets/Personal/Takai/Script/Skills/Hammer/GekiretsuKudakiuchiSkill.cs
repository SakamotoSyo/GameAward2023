using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class GekiretsuKudakiuchiSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    const float _subtractValue = 0.4f;

    public GekiretsuKudakiuchiSkill()
    {
        SkillName = "激烈・砕き打ち";
        Damage = 170;
        RequiredPoint = 5;
        Weapon = (WeaponType)2;
        Type = (SkillType)1;
        FlavorText = "敵の攻撃、会心率、素早さを40%下げる　※使用した武器は壊れる";
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
        _actor = actorType;
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        SkillEffect();
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        FluctuationStatusClass fluctuation;
        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                // スキルの効果処理を実装する
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);

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
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower + Damage);

                fluctuation =
                    new FluctuationStatusClass(-_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value * _subtractValue,
                        -_playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value * _subtractValue,
                        -_playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value * _subtractValue * _subtractValue, 0, 0);
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
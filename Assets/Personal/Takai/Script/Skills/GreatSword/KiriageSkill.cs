using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class KiriageSkill : SkillBase
{
    [SerializeField] private PlayableDirector _playerAnim;
    [SerializeField] private GameObject _playerObj;
    [SerializeField] private PlayableDirector _enemyRedAnim;
    [SerializeField] private GameObject _enemyRedObj;
    [SerializeField] private PlayableDirector _enemyBlueAnim;
    [SerializeField] private GameObject _enemyBlueObj;
    [SerializeField] private PlayableDirector _enemyGreenAnim;
    [SerializeField] private GameObject _enemyGreenObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private ActorAttackType _actor;
    private const float AddDamageValue = 0.25f;
    private const int Turn = 3;

    private int _playerCount = 0;
    private int _playerTurnCount;
    private float _playerBuffValue = 0;
    private int _enemyCount = 0;
    private int _enemyTurnCount;
    private float _enemyBuffValue = 0;

    public KiriageSkill()
    {
        SkillName = "斬り上げ";
        Damage = 1.2f;
        RequiredPoint = 15;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "2ターンの間、攻撃力が25%上昇。(重複あり→25%,50%,75%)";
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
            switch (_enemyStatus.EnemyColor)
            {
                case EnemyColor.Red:
                {
                    _enemyRedObj.SetActive(true);
                    _enemyStatus.gameObject.SetActive(false);
                    _enemyRedAnim.Play();
                    var dura = _enemyRedAnim.duration * 0.99f;
                    await UniTask.WaitUntil(() => _enemyRedAnim.time >= dura,
                        cancellationToken: this.GetCancellationTokenOnDestroy());
                    SkillEffect();
                    _enemyRedAnim.Stop();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                    _enemyStatus.gameObject.SetActive(true);
                    Debug.Log("Anim End");
                    _enemyRedObj.SetActive(false);
                }
                    break;
                case EnemyColor.Blue:
                {
                    _enemyBlueObj.SetActive(true);
                    _enemyStatus.gameObject.SetActive(false);
                    _enemyBlueAnim.Play();
                    var dura = _enemyBlueAnim.duration * 0.99f;
                    await UniTask.WaitUntil(() => _enemyBlueAnim.time >= dura,
                        cancellationToken: this.GetCancellationTokenOnDestroy());
                    SkillEffect();
                    _enemyBlueAnim.Stop();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                    _enemyStatus.gameObject.SetActive(true);
                    Debug.Log("Anim End");
                    _enemyBlueObj.SetActive(false);
                }
                    break;
                case EnemyColor.Green:
                {
                    _enemyGreenObj.SetActive(true);
                    _enemyStatus.gameObject.SetActive(false);
                    _enemyGreenAnim.Play();
                    var dura = _enemyGreenAnim.duration * 0.99f;
                    await UniTask.WaitUntil(() => _enemyGreenAnim.time >= dura,
                        cancellationToken: this.GetCancellationTokenOnDestroy());
                    SkillEffect();
                    _enemyGreenAnim.Stop();
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5));
                    _enemyStatus.gameObject.SetActive(true);
                    Debug.Log("Anim End");
                    _enemyGreenObj.SetActive(false);
                }
                    break;
            }
        }
    }

    protected override void SkillEffect()
    {
        switch (_actor)
        {
            case ActorAttackType.Player:
            {
                float dmg = _playerStatus.PlayerStatus.EquipWeapon.GetPowerPram();
                _enemyStatus.AddDamage(dmg * Damage, _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());

                if (++_playerCount <= Turn)
                {
                    FluctuationStatusClass fluctuation;
                    if (_playerCount != 0)
                    {
                        fluctuation = new FluctuationStatusClass(
                            -_playerBuffValue, 0, 0, 0, 0);
                        _playerBuffValue = 0;
                        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
                    }

                    _playerBuffValue = dmg * (AddDamageValue * _playerCount);
                    fluctuation = new FluctuationStatusClass(
                        _playerBuffValue,
                        0, 0, 0, 0);

                    _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
                }
            }
                break;
            case ActorAttackType.Enemy:
            {
                float dmg = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value;
                _playerStatus.AddDamage(dmg * Damage, _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value);

                if (++_enemyCount <= Turn)
                {
                    FluctuationStatusClass fluctuation;
                    if (_enemyCount != 0)
                    {
                        fluctuation = new FluctuationStatusClass(
                            -_enemyBuffValue, 0, 0, 0, 0);
                        _playerBuffValue = 0;
                        _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
                    }

                    _enemyBuffValue = dmg * (AddDamageValue * _enemyCount);
                    fluctuation = new FluctuationStatusClass(
                        _enemyBuffValue,
                        0, 0, 0, 0);

                    _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
                }
            }
                break;
        }
    }

    public override bool TurnEnd()
    {
        if (_playerCount > 0)
        {
            if (++_playerTurnCount >= Turn)
            {
                _playerCount--;

                FluctuationStatusClass fluctuation = new FluctuationStatusClass(
                    -_playerBuffValue, 0, 0, 0, 0);
                _playerBuffValue = 0;
                _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
            }
        }

        if (_enemyCount > 0)
        {
            if (++_enemyTurnCount >= Turn)
            {
                _enemyCount--;

                FluctuationStatusClass fluctuation = new FluctuationStatusClass(
                    -_enemyBuffValue, 0, 0, 0, 0);
                _enemyBuffValue = 0;
                _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
            }
        }

        return true;
    }


    public override void BattleFinish()
    {
        FluctuationStatusClass playerFluctuation = new FluctuationStatusClass(-_playerBuffValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(playerFluctuation);
        FluctuationStatusClass enemyFluctuation = new FluctuationStatusClass(-_enemyBuffValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(enemyFluctuation);

        _playerCount = 0;
        _playerBuffValue = 0;
        _enemyCount = 0;
        _enemyBuffValue = 0;
    }
}
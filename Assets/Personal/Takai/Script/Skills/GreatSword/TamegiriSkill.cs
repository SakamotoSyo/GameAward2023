using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class TamegiriSkill : SkillBase
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

    public TamegiriSkill()
    {
        SkillName = "溜め斬り";
        Damage = 60;
        RequiredPoint = 5;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "剣を大きく振りかぶる攻撃";
    }

    private void Start()
    {
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
        // スキルの効果処理を実装する
        switch (_actor)
        {
            case ActorAttackType.Player:
                _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
                break;
            case ActorAttackType.Enemy:
                _playerStatus.AddDamage(_enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower + Damage);
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
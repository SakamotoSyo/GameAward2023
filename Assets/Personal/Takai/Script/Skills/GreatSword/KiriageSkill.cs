using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.Playables;

public class KiriageSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private const float AddDamageValue = 0.05f;
    private const int Turn = 3;
    private int _count = 0;
    private int _turnCount;
    private float _buffValue = 0;

    public KiriageSkill()
    {
        SkillName = "斬り上げ";
        Damage = 70;
        RequiredPoint = 5;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "2ターンの間攻撃力が5%上昇。(重複あり→5%,10%,15%)";
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
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        SkillEffect();
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        float dmg = _playerStatus.PlayerStatus.EquipWeapon.GetPowerPram();
        _enemyStatus.AddDamage(dmg + Damage);

        if (++_count <= Turn)
        {
            FluctuationStatusClass fluctuation;
            if (_count != 0)
            {
                fluctuation = new FluctuationStatusClass(
                    -_buffValue, 0, 0, 0, 0);
                _buffValue = 0;
                _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
            }

            _buffValue = dmg * (AddDamageValue * _count);
            fluctuation = new FluctuationStatusClass(
                _buffValue,
                0, 0, 0, 0);

            _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
        }
    }

    public override bool TurnEnd()
    {
        if (_count > 0)
        {
            if (++_turnCount >= Turn)
            {
                _count--;

                FluctuationStatusClass fluctuation = new FluctuationStatusClass(
                    -_buffValue, 0, 0, 0, 0);
                _buffValue = 0;
                _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
            }
        }

        return true;
    }


    public override void BattleFinish()
    {
        FluctuationStatusClass fluctuation = new FluctuationStatusClass(-_buffValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);

        _count = 0;
        _buffValue = 0;
    }
}
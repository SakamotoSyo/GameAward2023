using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;


public class KasokuSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private const float ADD_VALUE = 0.05f;
    private const int Turn = 4;
    [NonSerialized]private int _count;
    [NonSerialized]private int _turnCount;
    [NonSerialized]private float _buffValue;

    public KasokuSkill()
    {
        SkillName = "加速";
        Damage = 0;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
        FlavorText = "3ターンの間重さが5%下降(重複あり→5%,10%,15%)";
    }

    private void Start()
    {
        _anim = GetComponent<PlayableDirector>();
    }

    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        float spd = _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value;

        if (++_count <= Turn)
        {
            FluctuationStatusClass fluctuation;
            if (_count != 0)
            {
                fluctuation = new FluctuationStatusClass(
                    0, -_buffValue, 0, 0, 0);
                _buffValue = 0;
                _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
            }

            _buffValue = spd * (ADD_VALUE * _count);
            fluctuation = new FluctuationStatusClass(
                0, _buffValue, 0, 0, 0);

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
                    0, -_buffValue, 0, 0, 0);
                _buffValue = 0;
                _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
            }
        }
        
        return true;
    }

    public override void BattleFinish()
    {
        FluctuationStatusClass fluctuation = new FluctuationStatusClass(0, -_buffValue, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
        
        _count = 0;
        _turnCount = 0;
        _buffValue = 0;
    }
}
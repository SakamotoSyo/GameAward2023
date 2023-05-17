using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KiaiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    [NonSerialized] private float _attackValue;
    [NonSerialized] private int _count;

    public KiaiSkill()
    {
        SkillName = "気合い";
        Damage = 0;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "次の攻撃だけ威力が2倍に上昇";
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
        _count++;

        _attackValue = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value;
        FluctuationStatusClass fluctuation =
            new FluctuationStatusClass(_attackValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
    }

    public override bool TurnEnd()
    {
        if (_count > 0)
        {
            _count--;
        }
        else
        {
            FluctuationStatusClass fluctuation =
                new FluctuationStatusClass(-_attackValue, 0, 0, 0, 0);
            _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
            _attackValue = 0;
        }

        return true;
    }

    public override void BattleFinish()
    {
        FluctuationStatusClass fluctuation =
            new FluctuationStatusClass(-_attackValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
        _attackValue = 0;
    }
}
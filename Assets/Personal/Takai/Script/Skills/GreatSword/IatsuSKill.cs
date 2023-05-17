using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IatsuSKill : SkillBase
{
    private PlayableDirector _anim;
    private EnemyController _enemyStatus;
    private const float PowerDown = 0.1f;
    [NonSerialized] private int _turn;
    [NonSerialized] private float _deBuffValue = 0;

    public IatsuSKill()
    {
        SkillName = "威圧";
        Damage = 0;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "2ターンの間敵の攻撃力を10%下げる";
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
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        float dmg = _enemyStatus.EnemyStatus.EquipWeapon.OffensivePower;
        if (_turn == 0)
        {
            _turn+=3;
            _deBuffValue += dmg * PowerDown;
            FluctuationStatusClass fluctuation = new FluctuationStatusClass(-_deBuffValue, 0, 0, 0, 0);
            _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
        }
        else
        {
            _turn += 3;
            Debug.Log("重複できません");
        }
    }

    public override bool TurnEnd()
    {
        if (--_turn <= 0)
        {
            FluctuationStatusClass fluctuation = new FluctuationStatusClass(_deBuffValue, 0, 0, 0, 0);
            _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
            _turn = 0;
            _deBuffValue = 0;
        }

        return true;
    }

    public override void BattleFinish()
    {
        FluctuationStatusClass fluctuation = new FluctuationStatusClass(_deBuffValue, 0, 0, 0, 0);
        _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
        _turn = 0;
        _deBuffValue = 0;
    }
}
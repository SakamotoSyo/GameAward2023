using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IatsuSKill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private const float PowerDown = 0.1f;
    private int _turn;
    private float _deBuffValue = 0;

    public IatsuSKill()
    {
        SkillName = "威圧";
        Damage = 0;
        RequiredPoint = 5;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "2ターンの間敵の攻撃力を10%下げる";
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
        // スキルの効果処理を実装する
        float dmg = _enemyStatus.EnemyStatus.EquipWeapon.OffensivePower;
        Debug.Log(dmg);
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
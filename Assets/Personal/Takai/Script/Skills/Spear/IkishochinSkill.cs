using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IkishochinSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private float _subtractValue = 0.2f;

    public IkishochinSkill()
    {
        SkillName = "意気消沈";
        Damage = 60;
        RequiredPoint = 20;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "敵の攻撃力20%を下げる";
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
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() + Damage);
        FluctuationStatusClass fluctuation =
            new FluctuationStatusClass(-_enemyStatus.EnemyStatus.EquipWeapon.OffensivePower + _subtractValue, 0, 0, 0,
                0);
        _enemyStatus.EnemyStatus.EquipWeapon.FluctuationStatus(fluctuation);
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
    }
}
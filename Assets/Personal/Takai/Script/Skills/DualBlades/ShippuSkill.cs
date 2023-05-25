using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class ShippuSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    const float _subtractHpValue = 0.5f;
    int _count = 3;

    public ShippuSkill()
    {
        SkillName = "疾風";
        Damage = 0.8f;
        RequiredPoint = 20;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
        FlavorText = "2ターンの間、敵に継続ダメージを与える。";
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
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * Damage,
            _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.GetPowerPram() * Damage,
            _playerStatus.PlayerStatus.EquipWeapon.GetCriticalPram());
        _count += 2;
    }

    public override bool TurnEnd()
    {
        if (_count <= 0)
        {
            _count--;
            float durable = _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value * _subtractHpValue;
            _enemyStatus.AddDamage(durable,0);
        }

        return true;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
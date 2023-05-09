
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class HauchiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    const float _subtractAttackValue = 0.2f;
    bool _isUse = false;
    public HauchiSkill()
    {
        SkillName = "刃打ち";
        Damage = 50;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "敵の攻撃力と会心率が20%下がる";
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        _isUse = true;
        // スキルの効果処理を実装する
        _playerStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
        _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower -= _enemyStatus.EnemyStatus.EquipWeapon.OffensivePower + _subtractAttackValue;
        _enemyStatus.EnemyStatus.EquipWeapon.CurrentCriticalRate -= _enemyStatus.EnemyStatus.EquipWeapon.CriticalRate * _subtractAttackValue;
    }

    public override void TurnEnd()
    {
        if (!_isUse)
        {
            return;
        }

        _isUse = false;
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class ShippuSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    private EnemyStatus _enemyStatus;
    const float _subtractHpValue = 0.02f;

    int _count = 3;

    public ShippuSkill()
    {
        SkillName = "疾風";
        Damage = 30;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _playerStatus.EquipWeapon.OffensivePower.Value += Damage;

        _count += 2;
    }

    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;

        if (_count <= 0)
        {
            _count--;
            float durable = _enemyStatus.EquipWeapon.CurrentDurable.Value;
            _enemyStatus.EquipWeapon.CurrentDurable.Value -= durable * _subtractHpValue;
        }
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
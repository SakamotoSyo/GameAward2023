using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IatsuSKill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private EnemyController _enemyStatus;
    private const float PowerDown = 0.1f;
    private const int Turn = 2;
    private int _turn;
    private float _attackValue = 0;

    public IatsuSKill()
    {
        SkillName = "威圧";
        Damage = 0;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
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
            _attackValue += dmg * PowerDown;
            _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower -= dmg * PowerDown;
        }
        else
        {
            Debug.Log("重複できません");
        }
    }

    public override void TurnEnd()
    {
        _turn++;
        if (_turn > Turn)
        {
            _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower += _attackValue;
            _turn = 0;
            _attackValue = 0;
        }
    }

    public override void BattleFinish()
    {
        _turn = 0;
        _attackValue = 0;
    }
}
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IatsuSKill : SkillBase
{
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
        FlavorText = "2ターンの間敵の攻撃力を10%下げる";
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

    public override bool TurnEnd()
    {
        _turn++;
        if (_turn > Turn)
        {
            _enemyStatus.EnemyStatus.EquipWeapon.CurrentOffensivePower += _attackValue;
            _turn = 0;
            _attackValue = 0;
        }

        return true;
    }

    public override void BattleFinish()
    {
        _turn = 0;
        _attackValue = 0;
    }
}
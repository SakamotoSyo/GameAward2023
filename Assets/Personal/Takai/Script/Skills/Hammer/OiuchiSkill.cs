using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class OiuchiSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private bool _isUse = false;

    public OiuchiSkill()
    {
        SkillName = "追い打ち";
        Damage = 45;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        _isUse = true;
        // スキルの効果処理を実装する
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage;
        if (_enemyStatus.EnemyStatus.IsDebuff()) //敵にデバフがついているか検知
        {
            _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage;
        }
    }

    public override void TurnEnd()
    {
        if (!_isUse)
        {
            return;
        }

        _isUse = false;
        
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= Damage;

        if (_enemyStatus.EnemyStatus.IsDebuff()) //敵にデバフがついているか検知
        {
            _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= Damage;
        }
    }

    public override void BattleFinish()
    {
        _isUse = false;
    }
}

using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KudakiuchiSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    const float _subtractValue = 0.2f;
    private bool _isSkill = false;
    bool _isUse = false;
    public KudakiuchiSkill()
    {
        SkillName = "砕き打ち";
        Damage = 50;
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
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        _isUse = true;
        // スキルの効果処理を実装する
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage;
        //敵の防御力を下げる処理
        if (!_isSkill)
        {
            _isSkill = true;
        }
    }

    public override void TurnEnd()
    {
        if (_isSkill)
        {
            float hp = _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value;
            _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value -= hp * _subtractValue;
        }

        if (!_isUse)
        {
            return;
        }

        _isUse = false;
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= Damage;
    }

    public override void BattleFinish()
    {
        _isUse = false;
        _isSkill = false;
    }
}
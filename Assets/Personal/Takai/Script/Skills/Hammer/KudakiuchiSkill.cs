
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
    private PlayerStatus _playerStatus;
    private EnemyStatus _enemyStatus;
    const float _subtractValue = 0.2f;
    private bool _isSkill = false;
    public KudakiuchiSkill()
    {
        SkillName = "砕き打ち";
        Damage = 50;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon, ActorAttackType actorType)
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
        // スキルの効果処理を実装する
        _playerStatus.EquipWeapon.OffensivePower.Value += Damage;
        //敵の防御力を下げる処理
        if (!_isSkill)
        {
            _isSkill = true;
        }
    }
    
    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;

        if(_isSkill)
        {
            float hp = _enemyStatus.EquipWeapon.CurrentDurable.Value;
            _enemyStatus.EquipWeapon.CurrentDurable.Value -= hp * _subtractValue;
        }
    }
    
    public override void BattleFinish()
    {
        _isSkill = false;   
    }
}
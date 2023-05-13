using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KudakiuchiSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    const float _subtractValue = 0.2f;
    private bool _isSkill = false;

    public KudakiuchiSkill()
    {
        SkillName = "砕き打ち";
        Damage = 50;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "攻撃した武器に継続ダメージを受ける状態を付与";
    }
    
    private void Start()
    {
        _anim = GetComponent<PlayableDirector>();
    }

    
    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);

        //敵の防御力を下げる処理
        if (!_isSkill)
        {
            _isSkill = true;
        }
    }

    public override bool TurnEnd()
    {
        if (_isSkill)
        {
            float hp = _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value;
            _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value -= hp * _subtractValue;
        }

        return false;
    }

    public override void BattleFinish()
    {
        _isSkill = false;
    }
}
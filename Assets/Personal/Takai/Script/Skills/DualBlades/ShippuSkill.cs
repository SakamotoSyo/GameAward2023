using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class ShippuSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    const float _subtractHpValue = 0.02f;
    int _count = 3;

    public ShippuSkill()
    {
        SkillName = "疾風";
        Damage = 30;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
        FlavorText = "2ターンの間敵に継続ダメージを与える";
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
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);

        _count += 2;
    }

    public override bool TurnEnd()
    {
        if (_count <= 0)
        {
            _count--;
            float durable = _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value;
            _enemyStatus.AddDamage(durable * _subtractHpValue);
        }

        return true;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
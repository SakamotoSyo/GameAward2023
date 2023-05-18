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
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
        _count += 2;
    }

    public override bool TurnEnd()
    {
        if (_count <= 0)
        {
            _count--;
            float durable = _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value* _subtractHpValue;
            _enemyStatus.AddDamage(durable);
        }

        return true;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
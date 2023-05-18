using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class OiuchiSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    
    public OiuchiSkill()
    {
        SkillName = "追い打ち";
        Damage = 45;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "威力が低いが、敵にデバフがついていると威力が倍になる";
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
        if (_enemyStatus.EnemyStatus.IsDebuff()) //敵にデバフがついているか検知
        {
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
        }
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {
        
    }
}
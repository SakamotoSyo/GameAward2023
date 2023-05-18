using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using System;

public class ShishifunjinSkill : SkillBase
{
    [Tooltip("攻撃間の待機時間")]
    [SerializeField] private int _attackWaitTime;
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private int _count ;

    public ShishifunjinSkill()
    {
        SkillName = "獅子奮迅";
        Damage = 20;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "経過ターンが多いほど攻撃回数アップ（上限 7回）";
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

    protected override async void SkillEffect()
    {
        var token = this.GetCancellationTokenOnDestroy();
        // スキルの効果処理を実装する
        _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + Damage);
        //経過ターンが多いほど攻撃回数アップ（上限 7回） 1 + 2×(ターン数-1) 
        int num = 1 + 2 * (_count - 1);
        if (7 < num) num = 7;

        for (int i = 0; i < num; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime), cancellationToken: token);
            _enemyStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value);
        }
    }

    public override bool TurnEnd()
    {
        _count++;

        return false;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
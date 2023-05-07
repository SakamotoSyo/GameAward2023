using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using System;

public class ShishifunjinSkill : SkillBase
{
    [Tooltip("攻撃間の待機時間")]
    [SerializeField] private int _attackWaitTime;
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private int _count;

    public ShishifunjinSkill()
    {
        SkillName = "獅子奮迅";
        Damage = 20;
        Weapon = (WeaponType)3;
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

    protected override async void SkillEffect()
    {
        var token = this.GetCancellationTokenOnDestroy();
        // スキルの効果処理を実装する
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value += Damage;
        //経過ターンが多いほど攻撃回数アップ（上限 7回） 1 + 2×(ターン数-1) 
        int num = 1 + 2 * (_count - 1);
        if(7 < num) num = 7;

        for (int i = 0; i < num; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_attackWaitTime), cancellationToken: token);
            _enemyStatus.EnemyStatus.EquipWeapon.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value);
        }
    }

    public override void TurnEnd()
    {
        _count++;
        _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value -= Damage;
    }

    public override void BattleFinish()
    {
        _count = 0;
    }
}
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;
using System;

public class ShishifunjinSkill : SkillBase
{
    [Tooltip("攻撃間の待機時間")]
    [SerializeField] private int _attackWaitTime;
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private int _count ;

    public ShishifunjinSkill()
    {
        SkillName = "獅子奮迅";
        Damage = 20;
        RequiredPoint = 5;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "経過ターンが多いほど攻撃回数アップ（上限 7回）";
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
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        SkillEffect();
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
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
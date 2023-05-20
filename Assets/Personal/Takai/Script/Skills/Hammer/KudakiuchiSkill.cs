using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KudakiuchiSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    const float _subtractValue = 0.2f;
    private bool _isSkill = false;

    public KudakiuchiSkill()
    {
        SkillName = "砕き打ち";
        Damage = 50;
        RequiredPoint = 5;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
        FlavorText = "攻撃した武器に継続ダメージを受ける状態を付与";
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
            float hp = _enemyStatus.EnemyStatus.EquipWeapon.CurrentDurable.Value * _subtractValue;
            _enemyStatus.AddDamage(hp);
        }

        return true;
    }

    public override void BattleFinish()
    {
        _isSkill = false;
    }
}
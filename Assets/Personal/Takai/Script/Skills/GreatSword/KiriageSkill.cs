using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KiriageSkill : SkillBase
{
    private PlayerController _playerStatus;
    private PlayableDirector _anim;
    private const float AddDamageValue = 0.05f;
    private const int Turn = 3;
    private int _count = 0;
    bool _isUse = false;
    public KiriageSkill()
    {
        SkillName = "斬り上げ";
        Damage = 70;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText= "2ターンの間攻撃力が5%上昇。(重複あり→5%,10%,15%)";
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        _isUse = true;

        float dmg = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value;
        // スキルの効果処理を実装する
        if (_count <= Turn)
        {
            _count++;
            _playerStatus.AddDamage(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value + (dmg * (AddDamageValue * _count)));
        }
    }

    public override void TurnEnd()
    {
        if (_isUse)
        {
            return;
        }

        _isUse = false;
    }


    public override void BattleFinish()
    {
        _isUse = false;
        _count = 0;
    }
}
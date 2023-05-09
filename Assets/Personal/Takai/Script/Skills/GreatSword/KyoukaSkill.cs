using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KyoukaSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private const float DamageFactor = 1.5f;
    private int _turn;

    public KyoukaSkill()
    {
        SkillName = "狂化";
        Damage = 0;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "次の技の攻撃力が1.5倍になる(重複なし)。攻撃後自ステータスが元に戻り、プレイヤーがひるむ";
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
        if (_turn == 0)
        {
            _turn++;
            _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value *= DamageFactor;
        }
        else
        {
            Debug.Log("重複できない");
        }
    }

    public override void TurnEnd()
    {
        _turn++;
        if (_turn > 2)
        {
            _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value /= DamageFactor;
            // プレイヤーがひるむ
            _turn = 0;
        }
    }

    public override void BattleFinish()
    {
        _turn = 0;
    }
}
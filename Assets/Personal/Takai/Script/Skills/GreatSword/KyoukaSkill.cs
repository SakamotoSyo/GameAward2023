using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KyoukaSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    private const float DamageFactor = 1.5f;
    private int _turn;

    public KyoukaSkill()
    {
        SkillName = "狂化";
        Damage = 0;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
        Debug.Log("Anim End");
    }
    
    protected override void SkillEffect()
    {
        if (_turn == 0)
        {
            _turn++;
            _playerStatus.EquipWeapon.OffensivePower.Value *= DamageFactor;
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
            _playerStatus.EquipWeapon.OffensivePower.Value /= DamageFactor;
            // プレイヤーがひるむ
            _turn = 0;
        }
    }

    public override void BattleFinish()
    {
        _turn = 0;
    }
}
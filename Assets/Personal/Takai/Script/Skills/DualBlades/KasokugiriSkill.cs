
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KasokugiriSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;

    public KasokugiriSkill()
    {
        SkillName = "加速斬り";
        Damage = 20;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon,ActorAttackType actorType)
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
        // スキルの効果処理を実装する
        _playerStatus.EquipWeapon.OffensivePower.Value += Damage;

        float weight = _playerStatus.EquipWeapon.WeaponWeight.Value;
        switch (Mathf.FloorToInt(weight / 10))
        {
            case 1:
                // 重さが20以下の場合の処理
                break;
            case 2:
                // 重さが21~30以下の場合の処理
                break;
            case 3:
                // 重さが31~40以下の場合の処理
                break;
            default:
                // 重さが40より大きい場合の処理
                break;
        }
    }


    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= Damage;
    }

    public override void BattleFinish()
    {

    }
}
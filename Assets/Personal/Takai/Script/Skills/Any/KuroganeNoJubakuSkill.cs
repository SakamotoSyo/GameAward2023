
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KuroganeNoJubakuSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;

    public KuroganeNoJubakuSkill()
    {
        SkillName = "黒鉄ノ呪縛";
        Damage = 0;
        Weapon = (WeaponType)4;
        Type = (SkillType)2;
        FlavorText = "攻撃力、重さ、会心率が40%上昇するが、受けるダメージが２倍になる。 武器が壊れるまで他の武器に変更できない。";
    }

    private void Start()
    {
        _anim = GetComponent<PlayableDirector>();
    }


    public override bool IsUseCheck(ActorGenerator actor)
    {
        return true;
    }


    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("くろがね");
        _playerStatus = player;
        _playerStatus.PlayerStatus.EquipWeapon.EpicSkill2();
        _anim = GetComponent<PlayableDirector>();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
    }

    public override bool TurnEnd()
    {
        return false;
    }

    public override void BattleFinish()
    {

    }
}
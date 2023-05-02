using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class NidangiriSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerStatus _playerStatus;
    float _attackValue = 0f;

    public NidangiriSkill()
    {
        SkillName = "2段斬り";
        Damage = 80;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
    }

    public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
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
        float dmg = _playerStatus.EquipWeapon.OffensivePower.Value;
        float weight = _playerStatus.EquipWeapon.WeaponWeight.Value;

        _attackValue += Damage;
        _playerStatus.EquipWeapon.OffensivePower.Value += Damage;

        if (weight >= 60)
        {
            _attackValue += 20;
            _playerStatus.EquipWeapon.OffensivePower.Value += 20;
        }
        else if (weight >= 50)
        {
            _attackValue += 15;
            _playerStatus.EquipWeapon.OffensivePower.Value += 15;
        }
        else if (weight >= 40)
        {
            _attackValue += 10;
            _playerStatus.EquipWeapon.OffensivePower.Value += 10;
        }
        else if (weight >= 30)
        {
            _attackValue += 5;
            _playerStatus.EquipWeapon.OffensivePower.Value += 5;
        }
    }

    public override void TurnEnd()
    {
        _playerStatus.EquipWeapon.OffensivePower.Value -= _attackValue;
        _attackValue = 0;
    }

    public override void BattleFinish()
    {
        _attackValue = 0;
    }
}
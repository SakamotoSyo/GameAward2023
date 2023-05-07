using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KasokuSkill : SkillBase
{
    public override string SkillName { get; protected set; }
    public override int Damage { get; protected set; }
    public override WeaponType Weapon { get; protected set; }
    public override SkillType Type { get; protected set; }
    public override string FlavorText { get; protected set; }
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private const float ADD_VALUE = 0.05f;
    float _speedValue = 0;
    private int _turn;
    private int _count = 0;

    public KasokuSkill()
    {
        SkillName = "加速";
        Damage = 0;
        Weapon = (WeaponType)1;
        Type = (SkillType)0;
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
        // スキルの効果処理を実装する
        float spd = _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value;
        
        _count++;
        _turn += 4;
        _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value -= _speedValue;
        _speedValue += (spd * (ADD_VALUE * _count));
        _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value += (spd * (ADD_VALUE * _count));
    }

    public override void TurnEnd()
    {
        float spd = _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value;
        
        _turn--;
        if (_turn <= 0)
        {
            _count = 0;
            _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value -= _speedValue;
        }
        else if(_turn <= 3)
        {
            _count--;
            _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value -= _speedValue;
            _speedValue += (spd * (ADD_VALUE * _count));
            _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value += (spd * (ADD_VALUE * _count));
        }
        else if(_turn <= 6)
        {
            _count--;
            _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value -= _speedValue;
            _speedValue += (spd * (ADD_VALUE * _count));
            _playerStatus.PlayerStatus.EquipWeapon.WeaponWeight.Value += (spd * (ADD_VALUE * _count));
        }
    }

    public override void BattleFinish()
    {
        _count = 0;
        _turn = 0;
        _speedValue = 0;
    }
}
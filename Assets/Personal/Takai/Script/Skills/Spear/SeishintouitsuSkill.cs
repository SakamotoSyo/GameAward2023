using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class SeishintouitsuSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    const float AddValue = 0.2f;
    private int _count;
    private float _value;
    private bool _isSkill = false;
    
    public SeishintouitsuSkill()
    {
        SkillName = "精神統一";
        Damage = 0;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "3ターンの間、会心率%と会心時のダメージが20%上昇(発動ターン含まず）";
    }
    
    private void Start()
    {
        _anim = GetComponent<PlayableDirector>();
    }

    
    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused, cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装する
        _count += 4;
        if(!_isSkill)
        {
            _isSkill = true;
            _value += _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value * (1 + AddValue);
            _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value += _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value * (1 + AddValue);
        }
        // 会心時のダメージが20%上昇

    }

    public override bool TurnEnd()
    {
        if (_count <= 0 && _isSkill)
        {
            _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value -= _value;
            _value = 0;
            _isSkill = false;
        }
        else 
        {
            _count--;
        }

        return false;
    }

    public override void BattleFinish()
    {
        _value = 0;
        _count = 0;
        _isSkill = false;
    }
}
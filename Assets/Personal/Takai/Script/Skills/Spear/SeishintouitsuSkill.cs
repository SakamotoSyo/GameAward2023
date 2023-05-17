using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class SeishintouitsuSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private const float ADD_VALUE = 0.2f;
    private const int TURN = 4;
    [NonSerialized] private int _count;
    [NonSerialized] private int _turnCount;
    [NonSerialized] private float _addValue;
    
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
        FluctuationStatusClass fluctuation;
        _count++;
        float value = _addValue;
        _addValue = _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value * ADD_VALUE;
        fluctuation = new FluctuationStatusClass(0, 0, -value, 0, 0);
        fluctuation = new FluctuationStatusClass(0, 0, _addValue, 0, 0);

    }

    public override bool TurnEnd()
    {
        if (_count > 0)
        {
            if (_turnCount <= TURN)
            {
                _turnCount++;
            }
            else
            {
                _count--;
                _turnCount = 0;
                float value = _addValue;
                FluctuationStatusClass  fluctuation = new FluctuationStatusClass(0, 0, -value, 0, 0);
                _addValue = _playerStatus.PlayerStatus.EquipWeapon.CriticalRate.Value * ADD_VALUE;
                if (_count <= 0)
                {
                    _count = 0;
                    _addValue = 0;
                }
            }
        }

        return true;
    }

    public override void BattleFinish()
    {
        _turnCount = 0;
        _addValue = 0;
    }
}
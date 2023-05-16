using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class IngaouhouSkill : SkillBase
{
    private PlayableDirector _anim;
    private PlayerController _playerStatus;
    private EnemyController _enemyStatus;
    private const float ADD_VALUE = 0.2f;
    private const int TURN = 4;
    private int _count;
    private int _turnCount;
    private float _addValue;

    public IngaouhouSkill()
    {
        SkillName = "因果応報";
        Damage = 0;
        Weapon = (WeaponType)3;
        Type = (SkillType)0;
        FlavorText = "発動したターンに攻撃を受けるとダメージを30%軽減し、反撃する。";
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
        _enemyStatus = enemy;
        _anim = GetComponent<PlayableDirector>();
        _anim.Play();
        SkillEffect();
        await UniTask.WaitUntil(() => _anim.state == PlayState.Paused,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        Debug.Log("Anim End");
    }

    protected override void SkillEffect()
    {
        // スキルの効果処理を実装す
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
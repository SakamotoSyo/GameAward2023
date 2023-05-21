using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public class KyoukaSkill : SkillBase
{
    [SerializeField] private PlayableDirector _anim;
    [SerializeField] private GameObject _playerObj;
    private PlayerController _playerStatus;
    private const float ADDVALUE = 0.5f;
    private int _turn = 0;
    private float _buffValue;

    public KyoukaSkill()
    {
        SkillName = "狂化";
        Damage = 0;
        RequiredPoint = 5;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
        FlavorText = "次の技の攻撃力が1.5倍になる(重複なし)。攻撃後自ステータスが元に戻り、プレイヤーがひるむ";
    }

    public override bool IsUseCheck(PlayerController player)
    {
        return true;
    }

    public async override UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType)
    {
        Debug.Log("Use Skill");
        _playerStatus = player;
        _playerObj.SetActive(true);
        _playerStatus.gameObject.SetActive(false);
        _anim.Play();
        var dura = _anim.duration * 0.99f;
        await UniTask.WaitUntil(() => _anim.time >= dura,
            cancellationToken: this.GetCancellationTokenOnDestroy());
        SkillEffect();
        _anim.Stop();
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        _playerStatus.gameObject.SetActive(true);
        Debug.Log("Anim End");
        _playerObj.SetActive(false);
    }

    protected override void SkillEffect()
    {
        FluctuationStatusClass fluctuation;

        _buffValue = _playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value * ADDVALUE;
        fluctuation = new FluctuationStatusClass(_buffValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
        Debug.Log(_playerStatus.PlayerStatus.EquipWeapon.OffensivePower.Value);
    }

    public override bool TurnEnd()
    {
        _turn++;
        Debug.Log(_turn);
        if (_turn >= 3)
        {
            _playerStatus.PlayerStatus.SetStateAnomaly(StateAnomaly.None);
            _turn = 0;
        }
        else if (_turn >= 2)
        {
            Debug.Log("スタン");
            FluctuationStatusClass fluctuation = new FluctuationStatusClass(-_buffValue, 0, 0, 0, 0);
            _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
            _buffValue = 0;
            // プレイヤーがひるむ
            _playerStatus.PlayerStatus.SetStateAnomaly(StateAnomaly.Stun);
        }
        
        return true;
    }

    public override void BattleFinish()
    {
        FluctuationStatusClass fluctuation = new FluctuationStatusClass(-_buffValue, 0, 0, 0, 0);
        _playerStatus.PlayerStatus.EquipWeapon.FluctuationStatus(fluctuation);
        _buffValue = 0;
        _turn = 0;
    }
}
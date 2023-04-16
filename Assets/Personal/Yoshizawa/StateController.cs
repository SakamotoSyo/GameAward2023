using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class StateController : MonoBehaviour
{
    /// <summary>戦闘時、キャラクターの行動順序を格納するリスト</summary>
    private List<GameObject> _orderOfAction = new List<GameObject>();
    public List<GameObject> OrderOfAction => _orderOfAction;

    private StateMachine<StateController> _stateMachine = null;
    public StateMachine<StateController> StateMachine => _stateMachine;

    private EnemyController[] _enemyController = null;
    public EnemyController[] EnemyController => _enemyController;
    public EnemyController CurrenyEnemy { get; private set; } = null;

    private PlayerController _playerController = null;
    public PlayerController PlayerController => _playerController;

    private ResultUIScript _resultUI = null;
    public ResultUIScript ResultUI => _resultUI;

    [SerializeField]
    private string _titleScene = "";
    public string TitleScene => _titleScene;
    [SerializeField]
    private string _homeScene = "";
    public string HomeScene => _homeScene;

    public enum TransitionCondition
    {
        Any2Start,
        Start2Player,
        Start2Enemy,
        Player2Enemy,
        Enemy2Enemy,
        Enemy2Player,
        Any2End,
    }

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _orderOfAction.Add(_playerController.gameObject);

        _enemyController = FindObjectsOfType<EnemyController>();

        for (int i = 0; i < _enemyController.Length; i++)
        {
            _orderOfAction.Add(_enemyController[i].gameObject);
        }

        _resultUI = FindObjectOfType<ResultUIScript>();
        
        _stateMachine = new(this);
        _stateMachine.AddAnyTranstion<BattleStartState>((int)TransitionCondition.Any2Start);
        _stateMachine.AddTransition<BattleStartState, PlayerAttackState>((int)TransitionCondition.Start2Player);
        _stateMachine.AddTransition<BattleStartState, EnemyAttackState>((int)TransitionCondition.Start2Enemy);
        _stateMachine.AddTransition<PlayerAttackState, EnemyAttackState>((int)TransitionCondition.Player2Enemy);
        _stateMachine.AddTransition<EnemyAttackState, EnemyAttackState>((int)TransitionCondition.Enemy2Enemy);
        _stateMachine.AddTransition<EnemyAttackState, PlayerAttackState>((int)TransitionCondition.Enemy2Player);
        _stateMachine.AddAnyTranstion<BattleEndState>((int)TransitionCondition.Any2End);

        // 始めに遷移するステート
        _stateMachine.Start<BattleStartState>();
    }

    private void Update()
    {
        if (BattleStartState.IsBattleCommandSelected)
        {
            // リストを所持武器の重さで並べ替えている
            _orderOfAction.Sort((a, b) =>
            {
                float x = 0f, y = 0f;
                
                // 変数xに代入する値の選定
                if (a.TryGetComponent(out EnemyController enemyA)) x = enemyA.EnemyStatus.EquipEnemyWeapon.WeaponWeight;
                else if (a.TryGetComponent(out PlayerController player)) x = player.PlayerStatus.EquipWeapon.WeaponWeight.Value;

                // 変数yに代入する値の選定
                if (b.TryGetComponent(out EnemyController enemyB)) y = enemyB.EnemyStatus.EquipEnemyWeapon.WeaponWeight;
                else if (b.TryGetComponent(out PlayerController player)) y = player.PlayerStatus.EquipWeapon.WeaponWeight.Value;

                if (x - y > 0)      return -1;
                else if (x - y < 0) return  1;
                else                return  0;
            });
        }

        _stateMachine.Update();
    }

    private int _count = 0;

    public void FirstStateTransition()
    {
        if (BattleStartState.IsBattleCommandSelected)
        {
            if (_orderOfAction[_count].TryGetComponent(out EnemyController enemyController))
            {
                _stateMachine.Dispatch((int)TransitionCondition.Start2Enemy);
            }
            else if (_orderOfAction[_count].TryGetComponent(out PlayerController playerController))
            {
                _stateMachine.Dispatch((int)TransitionCondition.Start2Player);
            }
        }
    }

    public void NextStateTransition()
    {
        GameObject currentActor = _orderOfAction[_count];

        if (_count < _orderOfAction.Count) _count++;
        else
        {
            _count = 0;
            TransitionToStartOrEnd(CurrenyEnemy.EnemyStatus.WeaponDates);
        }

        GameObject nextActor = _orderOfAction[_count];

        // 次に行動するActorがEnemyだったら
        if (nextActor.TryGetComponent(out EnemyController nextEnemy))
        {
            // 現在行動しているActorがEnemyだったら
            if (currentActor.TryGetComponent(out EnemyController currentEnemy))
            {
                _stateMachine.Dispatch((int)TransitionCondition.Enemy2Enemy);
            }
            // 現在行動しているActorがPlayerだったら
            else if (currentActor.TryGetComponent(out PlayerController currentPlayer))
            {
                CurrenyEnemy = nextEnemy;
                _stateMachine.Dispatch((int)TransitionCondition.Player2Enemy);
            }
        }
        // 次に行動するActorがPlayerだったら
        else if (nextActor.TryGetComponent(out PlayerController nextPlayer))
        {
            _stateMachine.Dispatch((int)TransitionCondition.Enemy2Player);
        }
    }

    /// <summary>戦闘開始 or 戦闘終了 のステートに遷移する</summary>
    /// <param name="weapons">所持武器の配列</param>
    public void TransitionToStartOrEnd(WeaponData[] weapons)
    {
        if (BrokenWeaponsCount(weapons) < weapons.Length)
        {
            _stateMachine.Dispatch((int)TransitionCondition.Any2Start);
        }
        else
        {
            _stateMachine.Dispatch((int)TransitionCondition.Any2End);
        }
    }

    /// <summary>壊れた武器の数を返す</summary>
    public int BrokenWeaponsCount(WeaponData[] weapons)
    {
        int count = 0;

        foreach (var weapon in weapons)
        {
            if (weapon.CurrentDurable <= 0f) count++;
        }

        return count;
    }
}

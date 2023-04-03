using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateController : MonoBehaviour
{
    public GameObject ComanndObj => _commandObj;

    [SerializeField] private GameObject _commandObj;
    [SerializeField] private ActorGenerator _generator;
    private StateMachine<BattleStateController> _stateMachine;
    private PlayerController _playerController;
    private List<EnemyController> _enemyController = new();
    

    void Start()
    {
        _playerController = _generator.PlayerController;
        _enemyController = _generator.EnemyControllerList;
        _stateMachine = new StateMachine<BattleStateController>(this);
        _stateMachine.Start<GameStartState>();
    }

    void Update()
    {
        _stateMachine.Update();
    }

    public enum BattleEvent
    {
        BattleStart,
        PlayerTurn,
        EnemyTurn,
        BattleEnd,
    }
}

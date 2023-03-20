using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateController : MonoBehaviour
{
    public GameObject ComanndObj => _commandObj;

    [SerializeField] private GameObject _commandObj;
    private StateMachine<BattleStateController> _stateMachine;

    void Start()
    {
        _stateMachine = new StateMachine<BattleStateController>(this);
        _stateMachine.Start<GameStartState>();
    }

    void Update()
    {
        
    }

    public enum BattleEvent
    {
        BattleStart,
        PlayerTurn,
        EnemyTurn,
        BattleEnd,
    }
}

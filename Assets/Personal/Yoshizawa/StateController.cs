using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class StateController : MonoBehaviour
{
    private StateMachine<StateController> _stateMachine = null;
    public StateMachine<StateController> StateMachine => _stateMachine;

    private void Awake()
    {
        _stateMachine = new(this);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        _stateMachine.Update();
    }
}

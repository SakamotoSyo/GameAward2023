using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class ActorStatusBase
{
    public IReactiveProperty<float> MaxHp => _maxHp;
    public IReactiveProperty<float> CurrentHp => _currentHp;

    protected ReactiveProperty<float> _maxHp = new();
    protected ReactiveProperty<float> _currentHp = new();

    public void Init() 
    {
        _maxHp.Value = 50;
        _currentHp.Value = 50;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class EnemyStatus
{
    private ReactiveProperty<float> _maxHp = new();

    private ReactiveProperty<float> _currentHp = new();

    private float _enemySpeed;

    public float GetSpeed => _enemySpeed;
}

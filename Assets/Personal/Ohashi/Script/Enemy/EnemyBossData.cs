using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyBossData
{
    public EnemyData EnemyData => _enemyData;
    [SerializeField] private EnemyData _enemyData;
    public bool BossFrag { set; get;}
}

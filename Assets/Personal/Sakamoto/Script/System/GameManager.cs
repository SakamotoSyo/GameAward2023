using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static EnemyData EnemyData => _enemyData;

    private static PlayerSaveData _saveData;
    private static EnemyData _enemyData;

    public static void SetEnemyData(EnemyData enemyData) 
    {
        _enemyData = enemyData;
    }
}

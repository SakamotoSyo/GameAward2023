using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static EnemyData EnemyData => _enemyData;
    public static PlayerSaveData PlayerSaveData => _saveData;

    private static PlayerSaveData _saveData;
    private static EnemyData _enemyData;
    private static WeaponType BlacksmithType;

    public static void SetEnemyData(EnemyData enemyData) 
    {
        _enemyData = enemyData;
    }

    public static void SetPlayerData(PlayerSaveData playerData) 
    {
       _saveData = playerData;
    }

    public static void SetBlacksmithType(WeaponType weaponType) 
    {
        BlacksmithType = weaponType;
    }
}

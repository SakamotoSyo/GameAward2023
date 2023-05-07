using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGenerator : MonoBehaviour
{
    public PlayerController PlayerController => _playerController;
    public EnemyController EnemyController => _enemyController;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private EnemyData _testEnemyData;
    [SerializeField] private Transform _playerInsPos;
    [SerializeField] private Transform _enemyInsPos;
    [SerializeField] private bool _isBossDebug = false;
    private PlayerController _playerController;
    private EnemyController _enemyController;

    private void Awake()
    {
        SetUp();
    }

    public void SetUp()
    {
        PlayerGeneration();
        EnemyGenetation(_testEnemyData.EnemyPrefab);
    }

    public void PlayerGeneration() 
    {
        var playerObj = Instantiate(_playerPrefab);
        playerObj.transform.SetParent(_playerInsPos.transform);
        _playerController = playerObj.GetComponent<PlayerController>();
    }

    public EnemyController EnemyGenetation(GameObject enemyPrefab)
    {
        var enemyObj = Instantiate(enemyPrefab);
        enemyObj.transform.SetParent(_enemyInsPos.transform);
        _enemyController = enemyObj.GetComponent<EnemyController>();
        if (_isBossDebug) 
        {
            _enemyController.EnemyStatus.IsBoss = true;
        }

        if (GameManager.EnemyData)
        {
            Debug.Log(GameManager.EnemyData.WeaponDates[0].WeaponType);
            _enemyController.SetEnemyData(GameManager.EnemyData);
        }
        else 
        {
            _enemyController.SetEnemyData(_testEnemyData);
        }
        
        return _enemyController;
    }
}

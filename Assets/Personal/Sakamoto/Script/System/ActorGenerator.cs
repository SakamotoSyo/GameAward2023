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
        if (GameManager.EnemyData)
        {
            var enemyData = GameManager.EnemyData;  
            var enemyObj = Instantiate(enemyData.EnemyPrefab, _enemyInsPos.transform.position, enemyData.EnemyPrefab.transform.rotation);
            enemyObj.transform.SetParent(_enemyInsPos.transform);
            _enemyController = enemyObj.GetComponent<EnemyController>();
            _enemyController.SetEnemyData(_testEnemyData);
            SoundManager.Instance.CriAtomBGMPlay("BGM_Battle");
        }
        else 
        {
            var enemyObj = Instantiate(enemyPrefab, _enemyInsPos.transform.position, enemyPrefab.transform.rotation);
            enemyObj.transform.SetParent(_enemyInsPos.transform);
            _enemyController = enemyObj.GetComponent<EnemyController>();
            _enemyController.SetEnemyData(_testEnemyData);
            SoundManager.Instance.CriAtomBGMPlay("BGM_Battle");
        }

        if (_isBossDebug) 
        {
            _enemyController.EnemyStatus.IsBoss = true;
        }

        if (GameManager.EnemyData && GameManager.EnemyData.IsBoss)
        {
            SoundManager.Instance.CriAtomBGMPlay("BGM_BossBattle");
            _enemyController.EnemyStatus.IsBoss = true;
        }

        _enemyController.StatusActive();
        return _enemyController;
    }
}

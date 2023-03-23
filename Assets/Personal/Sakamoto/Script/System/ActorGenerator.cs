using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGenerator : MonoBehaviour
{
    public PlayerController PlayerController => _playerController;
    public List<EnemyController> EnemyControllerList => _enemyControllerList;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerInsPos;
    private PlayerController _playerController;
    //ToDo:Œã‚ÅEnemyController‚É•Ï‚¦‚é
    private List<EnemyController> _enemyControllerList = new();

    private void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        PlayerGeneration();
    }

    public void PlayerGeneration() 
    {
        var playerObj = Instantiate(_playerPrefab);
        playerObj.transform.SetParent(_playerInsPos.transform);
        _playerController = playerObj.GetComponent<PlayerController>();
    }

    public void EnemyGenetation(List<GameObject> enemyList)
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            _enemyControllerList.Add(enemyList[i].GetComponent<EnemyController>());
        }
    }
}

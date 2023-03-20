using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerInsPos;
    private PlayerController _playerController;
    //ToDo:Œã‚ÅEnemyController‚É•Ï‚¦‚é
    private List<ParticleTest> _enemyController;

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

    public void EnemyGenetation(List<ParticleTest> enemyList) 
    {
        //for(int i = 0; )
    }
}

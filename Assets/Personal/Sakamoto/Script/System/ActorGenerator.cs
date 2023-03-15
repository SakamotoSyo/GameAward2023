using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _PlayerPrefab;
    [SerializeField] private Transform _playerInsPos;
    private PlayerController _playerController;
    //ToDo:Œã‚ÅEnemyController‚É•Ï‚¦‚é
    private List<ParticleTest> _enemyController;

    public void SetUp()
    {
        PlayerGeneration();
    }

    public void PlayerGeneration() 
    {
        var playerObj = Instantiate(_PlayerPrefab);
        _PlayerPrefab.transform.SetParent(playerObj.transform);
        _playerController = _playerController.GetComponent<PlayerController>();
    }

    public void EnemyGenetation(List<ParticleTest> enemyList) 
    {
        //for(int i = 0; )
    }
}

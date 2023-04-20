using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingBattleScript : MonoBehaviour
{
    [SerializeField] private EnemyDataBase _enemyDataBase;
    [SerializeField] private Button[] _selectButton = new Button[3];
    [SerializeField] private Image[] _enemyImage = new Image[3];
    [SerializeField] private int _rankPointRange = 100;
    [SceneName]
    [SerializeField] private string _battleScene;
    

    void Start()
    {
        StartEnemySelect();
    }

    void Update()
    {

    }

    public void StartEnemySelect() 
    {
        if (GameManager.PlayerSaveData == null) 
        {
            var playerData = new PlayerSaveData();
            playerData.PlayerRankPoint = 0;
            GameManager.SetPlayerData(playerData);
        }
        for (int i = 0; i < _selectButton.Length; i++)
        {
            var enemyData = _enemyDataBase.GetRandomEnemyData(GameManager.PlayerSaveData.PlayerRankPoint, _rankPointRange);
            _enemyImage[i].sprite = enemyData.EnemySprite;
            _selectButton[i].onClick.AddListener(() => GameManager.SetEnemyData(enemyData));
            _selectButton[i].onClick.AddListener(() => SceneLoader.LoadScene(_battleScene));
        }
    }

}

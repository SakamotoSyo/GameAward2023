using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingBattleScript : MonoBehaviour
{
    [SerializeField] private EnemyDataBase _enemyDataBase;
    [SerializeField] private GameObject _buttonInsPos;
    [SerializeField] private GameObject _enemyPrefab;
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
            PlayerDataInit();
        }
        EnemyData[] enemyData = _enemyDataBase.GetEnemyArrayData(GameManager.PlayerSaveData.PlayerRankPoint, _rankPointRange);
        for (int i = 0; i < enemyData.Length; i++)
        {
            var enemyButtonPrefab = Instantiate(_enemyPrefab);
            enemyButtonPrefab.transform.SetParent(_buttonInsPos.transform);
            enemyButtonPrefab.GetComponent<Image>().sprite = enemyData[i].EnemySprite;
            var button = enemyButtonPrefab.GetComponent<Button>();
            button.onClick.AddListener(() => GameManager.SetEnemyData(enemyData[i]));
            button.onClick.AddListener(() => SceneLoader.LoadScene(_battleScene));
        }
    }


    public void PlayerDataInit() 
    {
        var playerData = new PlayerSaveData();
        var weaponDatas = new WeaponData[4];
        for (int i = 0; i < 4; i++)
        {
            weaponDatas[i] = new WeaponData(1000, 1000, 50, 1000, WeaponData.AttributeType.None, WeaponType.GreatSword);
        }
        playerData.WeaponArray = weaponDatas;
        playerData.PlayerRankPoint = 0;
        GameManager.SetPlayerData(playerData);
    }

}

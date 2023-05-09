using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RankingBattleScript : MonoBehaviour
{
    [SerializeField] private EnemyDataBase _enemyDataBase;
    [SerializeField] private GameObject _buttonInsPos;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _bossButtonObj;
    [SerializeField] private Text _rankText;
    [SerializeField] private int _rankPointRange = 100;
    [SceneName]
    [SerializeField] private string _battleScene;
    private List<EnemyData> _enemyBossData = new();

    private void Awake()
    {
        PlayerWeaponRecovery();
    }

    void Start()
    {
        StartEnemySelect();
        BossChallengeJudge();
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
        EnemyData[] enemyDataHigh = _enemyDataBase.GetRankEnemyArrayData(GameManager.PlayerSaveData.PlayerRankPoint, _rankPointRange, true);
        for (int i = 0; i < enemyDataHigh.Length; i++)
        {
            var enemyButtonPrefab = Instantiate(_enemyPrefab);
            enemyButtonPrefab.transform.SetParent(_buttonInsPos.transform);
            var randomIndex = Random.Range(0, enemyDataHigh.Length);
            //enemyButtonPrefab.GetComponent<Image>().sprite = enemyData[i].EnemySprite;
            enemyButtonPrefab.GetComponent<Image>().sprite = enemyDataHigh[randomIndex].EnemySprite;
            var button = enemyButtonPrefab.GetComponent<Button>();

            button.onClick.AddListener(() => SelectEnemy.SetImage(enemyButtonPrefab.GetComponent<Image>().sprite));
            //button.onClick.AddListener(() => GameManager.SetEnemyData(enemyData[i]));
            button.onClick.AddListener(() => GameManager.SetEnemyData(enemyDataHigh[randomIndex]));
            //button.onClick.AddListener(() => SceneLoader.LoadScene(_battleScene));
        }

        //EnemyData[] enemyDataLow = _enemyDataBase.GetRankEnemyArrayData(GameManager.PlayerSaveData.PlayerRankPoint, _rankPointRange, false);
        //for (int i = 0; i < 2; i++)
        //{
        //    var enemyButtonPrefab = Instantiate(_enemyPrefab);
        //    enemyButtonPrefab.transform.SetParent(_buttonInsPos.transform);
        //    var randomIndex = Random.Range(0, enemyDataLow.Length);
        //    //enemyButtonPrefab.GetComponent<Image>().sprite = enemyData[i].EnemySprite;
        //    enemyButtonPrefab.GetComponent<Image>().sprite = enemyDataLow[randomIndex].EnemySprite;
        //    var button = enemyButtonPrefab.GetComponent<Button>();

        //    button.onClick.AddListener(() => SelectEnemy.SetImage(enemyButtonPrefab.GetComponent<Image>().sprite));
        //    button.onClick.AddListener(() => GameManager.SetEnemyData(enemyDataLow[randomIndex]));
        //}
    }


    public void BossChallengeJudge()
    {
        var enemyData = EnemyDataBase.BossSelect();
        if (enemyData != null)
        {
            _enemyBossData.Add(enemyData);
        }

        if (_enemyBossData.Count != 0)
        {
            _bossButtonObj.SetActive(true);
            var botton = _bossButtonObj.GetComponent<Button>();
            botton.onClick.AddListener(() => ButtonBossSelect());
        }
    }

    public void ButtonBossSelect()
    {
        GameManager.SetEnemyData(_enemyBossData[0]);
        SceneLoader.LoadScene(_battleScene);
    }

    public void PlayerWeaponRecovery()
    {
        if (GameManager.PlayerSaveData == null) return;

        var weaponDatas = GameManager.PlayerSaveData.WeaponArray;

        for (int i = 0; i < weaponDatas.Length; i++)
        {
            weaponDatas[i].CurrentDurable = weaponDatas[i].MaxDurable;
        }
        GameManager.PlayerSaveData.WeaponArray = weaponDatas;
        _rankText.text = GameManager.PlayerSaveData.PlayerRankPoint.ToString();
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

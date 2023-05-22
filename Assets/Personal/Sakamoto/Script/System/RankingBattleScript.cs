using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingBattleScript : MonoBehaviour
{
    [SerializeField] private Image _bossImage = default;
    [SerializeField] private Sprite[] _rankSprite = new Sprite[5];

    [SerializeField] private EnemyDataBase _enemyDataBase;
    [SerializeField] private GameObject _buttonInsPos;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _bossButtonObj;
    [SerializeField] private int _rankPointRange = 100;
    [SceneName]
    [SerializeField] private string _battleScene;
    private List<EnemyData> _enemyBossData = new();

    private void Awake()
    {
        PlayerWeaponRecovery();
    }

    private void Start()
    {
        StartEnemySelect();
        BossChallengeJudge();
    }

    public void StartEnemySelect()
    {
        if (GameManager.PlayerSaveData == null)
        {
            PlayerDataInit();
        }
        EnemyData[] enemyDataHigh
            = _enemyDataBase.GetRankEnemyArrayData(GameManager.PlayerSaveData.PlayerRankPoint, _rankPointRange, true);

        for (int i = 0; i < 5; i++)
        {
            var enemyButtonPrefab = Instantiate(_enemyPrefab);
            enemyButtonPrefab.transform.SetParent(_buttonInsPos.transform);
            var button = enemyButtonPrefab.GetComponent<Button>();

            int index = 0;

            if (i == 0)
            {
                //è∏äiêÌÇÃspriteÇàÍî‘è„Ç…äÑÇËìñÇƒÇÈ
                index = HomeScene.Instance.IsChallengablePromotionMatch ? 4 : Random.Range(0, _rankSprite.Length - 1);
            }
            else
            {
                index = Random.Range(0, _rankSprite.Length - 1);
            }
            enemyButtonPrefab.GetComponent<Image>().sprite = _rankSprite[index];

            var randomIndex = Random.Range(0, enemyDataHigh.Length);

            if (i == 0)
            {
                _bossImage.sprite = enemyDataHigh[randomIndex].EnemySprite;
            }

            button.onClick.AddListener(() => SelectEnemy.SetImage(enemyDataHigh[randomIndex].EnemySprite));
            button.onClick.AddListener(() => GameManager.SetEnemyData(enemyDataHigh[randomIndex]));
        }
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
        Debug.Log("Ç´ÇΩ");
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

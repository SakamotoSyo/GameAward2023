using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingBattleScript : MonoBehaviour
{
    [SerializeField] private Image _bossImage = default;
    [SerializeField] private Sprite[] _rankSprite = new Sprite[5];
    [SerializeField] private Text _flavorText = default;

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
                //¸Šií‚Ìsprite‚ðˆê”Ôã‚ÉŠ„‚è“–‚Ä‚é
                if (HomeScene.Instance.IsChallengablePromotionMatch)
                {
                    index = 4;
                }
                else
                {
                    index = PlayerExperiencePoint.CurrentRankNum switch
                    {
                        0 => 1,
                        1 => 2,
                        2 => 3,
                        3 => 3,
                        _ => 0,
                    };
                }
            }
            else
            {
                //‚±‚±’¼‚·
                switch (PlayerExperiencePoint.CurrentRankNum)
                {
                    case 0:
                        index =
                            i == 1 ? 1 : 0;
                        break;
                    case 1:
                        index =
                            i == 4 ? 0 : 1;
                        break;
                    case 2:
                        index =
                            i == 4 ? 2 : 1;
                        break;
                    case 3:
                        index =
                            i == 1 ? 3 : 2;
                        break;
                }
            }
            enemyButtonPrefab.GetComponent<Image>().sprite = _rankSprite[index];

            if (enemyDataHigh.Length != 0)
            {
                var randomIndex = Random.Range(0, enemyDataHigh.Length);

                if (i == 0)
                {
                    _bossImage.sprite = PlayerExperiencePoint.CurrentRankNum switch
                    {
                        0 => EnemyDataBase.BossDataList[0].EnemyData.EnemySprite,
                        1 => EnemyDataBase.BossDataList[1].EnemyData.EnemySprite,
                        2 => EnemyDataBase.BossDataList[2].EnemyData.EnemySprite,
                        _ => default
                    };

                    if (HomeScene.Instance.IsChallengablePromotionMatch)
                    {
                        button.onClick.AddListener(
                            () => SelectEnemy.SetImage(enemyDataHigh[PlayerExperiencePoint.CurrentRankNum].EnemySprite, true));
                        button.onClick.AddListener(
                            () => GameManager.SetEnemyData(enemyDataHigh[PlayerExperiencePoint.CurrentRankNum]));
                        button.onClick.AddListener(
                            () => SetFlavorText(enemyDataHigh[PlayerExperiencePoint.CurrentRankNum].FlavorText));

                        continue;
                    }
                    else
                    {
                        button.onClick.AddListener(
                            () => SelectEnemy.SetImage(enemyDataHigh[PlayerExperiencePoint.CurrentRankNum].EnemySprite, false));
                        button.onClick.AddListener(
                            () => GameManager.SetEnemyData(enemyDataHigh[PlayerExperiencePoint.CurrentRankNum]));
                        button.onClick.AddListener(
                            () => SetFlavorText(enemyDataHigh[PlayerExperiencePoint.CurrentRankNum].FlavorText));
                    }
                }

                button.onClick.AddListener(() => SelectEnemy.SetImage(enemyDataHigh[randomIndex].EnemySprite, false));
                button.onClick.AddListener(() => GameManager.SetEnemyData(enemyDataHigh[randomIndex]));
                button.onClick.AddListener(() => SetFlavorText(enemyDataHigh[randomIndex].FlavorText));
            }
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
        Debug.Log("‚«‚½");
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

    private void SetFlavorText(string text)
    {
        _flavorText.text = text;
    }
}

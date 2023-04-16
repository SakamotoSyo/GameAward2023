using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIScript : MonoBehaviour
{
    [SerializeField] private OreUIScript[] _oreUiCs = new OreUIScript[3];
    [SerializeField] private Button[] _oreButtonUi = new Button[3];
    [SerializeField] private GameObject _oreSelectObj;
    [SerializeField] private GameObject _enhanceSelectObj;
    [SerializeField] private ActorGenerator _actorGenerator;
    [SerializeField] private PlayerSkillDataManagement _playerSkillDataManagement;
    [SerializeField] private Image[] _oreImage = new Image[3];
    [SerializeField] private ResultWeaponButton[] _resultUICs = new ResultWeaponButton[4];
    [Tooltip("リザルトシーンのテスト用")]
    [SerializeField] private bool _resultSceneTest = false;


    void Start()
    {
        RewardLottery();
    }

    void Update()
    {

    }

    /// <summary>
    /// 報酬の抽選をしてデータを渡す
    /// </summary>
    private void RewardLottery()
    {
        for (int i = 0; i < _oreUiCs.Length; i++)
        {
            var oreInfo = RarityLottery();
            Debug.Log(oreInfo.randomSkill);
            OreData Ore = new OreData(SetEnhanceData(), oreInfo.rearity, oreInfo.randomSkill, oreInfo.oreImage);
            _oreUiCs[i].SetData(Ore);
            _oreButtonUi[i].onClick.AddListener(() => EnhanceWeaponEvent(Ore));
        }
    }

    /// <summary>
    /// 強化するStatusを返してくれる
    /// </summary>
    /// <returns></returns>
    private EnhanceData[] SetEnhanceData()
    {
        EnhanceData[] enhanceData = new EnhanceData[DataBaseScript.EnhanceData.Count];
        for (int i = 0; i < DataBaseScript.EnhanceData.Count; i++)
        {
            //そのステータスを強化するかどうか
            int enhanceBool = Random.Range(0, 2);
            if (enhanceBool == 0)
            {
                //強化する数字
                int enhanceNum = Random.Range(1, 3);
                enhanceData[i].EnhanceNum = enhanceNum;
                enhanceData[i].EnhanceDescription = DataBaseScript.EnhanceData[i].EnhanceDescription;
            }
        }
        return enhanceData;
    }

    private (Image oreImage, OreRarity rearity, SkillBase randomSkill) RarityLottery() 
    {
       OreRarity rearity = (OreRarity)Random.Range(0, 3);
       //WeaponData[] weaponDatas = _actorGenerator.PlayerController.PlayerStatus.WeponDatas;
       //WeaponData weaponData = weaponDatas[Random.Range(0, weaponDatas.Length)];
       //TODO:テストで動かせるようになっているのでPlayerが生成される環境だったら上を使う
       //WeaponType weaponType = (WeaponType)Random.Range(0, 4);
       SkillBase randomSkill = _playerSkillDataManagement.OnSkillCall(WeaponType.GreatSword, OreRarity.Normal);
       Image oreImage = _oreImage[(int)rearity];

        return (oreImage, rearity, randomSkill); 
    }

    /// <summary>
    ///鉱石を選んだ際にどの武器に鉱石を混ぜるか選択できるEventに移る
    /// </summary>
    public void EnhanceWeaponEvent(OreData selectOreData)
    {
        _oreSelectObj.SetActive(false);
        _enhanceSelectObj.SetActive(true);
        WeaponData[] weaponDatas;
        if (_resultSceneTest)
        {
            weaponDatas = new WeaponData[4];
            for (int i = 0; i < weaponDatas.Length; i++)
            {
                weaponDatas[i] = new(50, 50, 50, 50, WeaponData.AttributeType.None, WeaponType.GreatSword);
            }
        }
        else 
        {
            weaponDatas = _actorGenerator.PlayerController.PlayerStatus.WeaponDatas;
        }

        for (int i = 0; i < weaponDatas.Length; i++) 
        {
            _resultUICs[i].SetUpInfo(weaponDatas[i], selectOreData);
        }

    }

    public void SelectAgain()
    {
        _oreSelectObj.SetActive(true);
        _enhanceSelectObj.SetActive(false);
    }
}

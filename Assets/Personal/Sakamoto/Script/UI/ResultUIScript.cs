using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIScript : MonoBehaviour
{
    [SerializeField] OreUIScript[] _oreUiCs = new OreUIScript[3];
    [SerializeField] ActorGenerator _actorGenerator;
    [SerializeField] PlayerSkillDataManagement _playerSkillDataManagement;
    [SerializeField] Image[] _oreImage = new Image[3];
    // Start is called before the first frame update
    void Start()
    {
        RewardLottery();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 報酬の抽選
    /// </summary>
    private void RewardLottery()
    {
        for (int i = 0; i < _oreUiCs.Length; i++)
        {
            var oreInfo = RarityLottery();
            OreData Ore = new OreData(SetEnhanceData(), oreInfo.rearity, oreInfo.randomSkill, oreInfo.oreImage);
            _oreUiCs[i].SetData(Ore);
        }
    }

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

    private (Image oreImage, OreRarity rearity, ISkillBase randomSkill) RarityLottery() 
    {
       OreRarity rearity = (OreRarity)Random.Range(0, 3);
       //WeaponData[] weaponDatas = _actorGenerator.PlayerController.PlayerStatus.WeponDatas;
       //WeaponData weaponData = weaponDatas[Random.Range(0, weaponDatas.Length)];
       //TODO:テストで動かせるようになっているのでPlayerが生成される環境だったら上を使う
       //WeaponType weaponType = (WeaponType)Random.Range(0, 4);
       ISkillBase randomSkill = _playerSkillDataManagement.OnSkillCall(WeaponType.GreatSword, OreRarity.Normal);
       Image oreImage = _oreImage[(int)rearity];

        return (oreImage, rearity, randomSkill); 
    }
}

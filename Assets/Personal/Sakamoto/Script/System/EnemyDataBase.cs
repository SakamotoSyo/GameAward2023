using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDataBase : MonoBehaviour
{
    [SerializeField] private List<EnemyData> _enemyDataListA = new();
    [SerializeField] private List<EnemyData> _enemyDataListB = new();
    [SerializeField] private List<EnemyData> _enemyDataListC = new();

    [SerializeField] private List<EnemyBossData> _bossDataList = new();


    private static bool isInit = false;
    private static List<EnemyBossData> bossDataList = new();

    public static List<EnemyBossData> BossDataList => bossDataList;

    private void Start()
    {
        if (!isInit)
        {
            Init();
        }
    }

    private void Init()
    {
        isInit = true;
        bossDataList = new List<EnemyBossData>(_bossDataList);
    }

    /// <summary>
    /// ƒ‰ƒ“ƒN‚ª‹ß‚¢ƒ‰ƒ“ƒ_ƒ€‚ÈEnemy‚ÌData‚ð•Ô‚·
    /// </summary>
    /// <returns></returns>
    public EnemyData[] GetRankEnemyArrayData(int playerRank, int RankRange, bool isUp)
    {
        EnemyData[] rankArray;
        var dataList = PlayerExperiencePoint.CurrentRankNum switch
        {
            0 => _enemyDataListC,
            1 => _enemyDataListB,
            2 => _enemyDataListA,
            _ => _enemyDataListA
        };

        if (isUp)
        {

            rankArray = dataList.Where(x => playerRank <= x.RankPoint
                                     && playerRank + RankRange >= x.RankPoint).ToArray();
        }
        else
        {
            rankArray = dataList.Where(x => playerRank - RankRange <= x.RankPoint
                                     && playerRank >= x.RankPoint).ToArray();
        }

        if (rankArray.Length == 0) 
        {
            //rankArray = dataList.Where(x => playerRank - RankRange <= x.RankPoint
            //                         && playerRank >= x.RankPoint).ToArray();
            rankArray = dataList.ToArray();
        }

        return rankArray;
    }

    public static EnemyData BossSelect()
    {
        int playerRankPoint = GameManager.PlayerSaveData.PlayerRankPoint;

        for (int i = 0; i < BossDataList.Count; i++) 
        {
            if (BossDataList[i].BossFrag == false && BossDataList[i].EnemyData.RankPoint < playerRankPoint) 
            {
                BossDataList[i].BossFrag = true;
                return BossDataList[i].EnemyData;
            }
        }

        return default;
    }
}

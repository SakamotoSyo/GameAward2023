using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDataBase : MonoBehaviour
{
    public static List<EnemyBossData> BossDataList => bossDataList;
    [SerializeField] private List<EnemyData> _enemyDataList = new();
    [SerializeField] private List<EnemyBossData> _bossDataList = new();
    private static bool isInit = false;
    private static List<EnemyBossData> bossDataList = new();

    private void Start()
    {
        if (!isInit)
        {
            Init();
        }
    }

    private void Init()
    {
        bossDataList = new List<EnemyBossData>(_bossDataList);
    }

    /// <summary>
    /// ƒ‰ƒ“ƒN‚ª‹ß‚¢ƒ‰ƒ“ƒ_ƒ€‚ÈEnemy‚ÌData‚ð•Ô‚·
    /// </summary>
    /// <returns></returns>
    public EnemyData[] GetRankEnemyArrayData(int playerRank, int RankRange, bool isUp)
    {
        EnemyData[] rankArray;
        if (isUp)
        {
            rankArray = _enemyDataList.Where(x => playerRank <= x.RankPoint
                                     && playerRank + RankRange >= x.RankPoint).ToArray();
        }
        else
        {
            rankArray = _enemyDataList.Where(x => playerRank - RankRange <= x.RankPoint
                                     && playerRank >= x.RankPoint).ToArray();
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

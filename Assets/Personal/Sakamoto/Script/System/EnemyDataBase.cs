using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDataBase : MonoBehaviour
{
   [SerializeField] private List<EnemyData> _enemyDataList = new();
   

    /// <summary>
    /// ƒ‰ƒ“ƒN‚ª‹ß‚¢ƒ‰ƒ“ƒ_ƒ€‚ÈEnemy‚ÌData‚ð•Ô‚·
    /// </summary>
    /// <returns></returns>
    public EnemyData GetRandomEnemyData(int playerRank, int RankRange) 
    {
        var rankList = _enemyDataList.Where(x => playerRank - RankRange <= x.RankPoint 
                                           && playerRank + RankRange >= x.RankPoint).ToList();
        return rankList[Random.Range(0, rankList.Count)];
    }

}

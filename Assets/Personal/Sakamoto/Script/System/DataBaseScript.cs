using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataBaseScript : MonoBehaviour
{
    public static List<EnhanceData> DescriptionEnhanceData => _enhanceData;

    [Header("レベルアップデータ")]
    [SerializeField] private TextAsset _levelUpTable;
    [SerializeField] private TextAsset _descriptionData;
    private static Dictionary<int, int> _statusData = new Dictionary<int, int>();
    private static List<EnhanceData> _enhanceData = new();

    private void Awake()
    {
        //LoadingLevelData();
        if (_enhanceData.Count == 0)
        {
            SetDescription();
        }
        
    }

    /// <summary>
    /// シーンの初めにレベルアップのデータを読み込む
    /// </summary>
    private void LoadingLevelData()
    {
        //テキストの読み込み
        StringReader sr = new StringReader(_levelUpTable.text);
        //最初の一行目はスキップ
        sr.ReadLine();

        while (true)
        {
            //一行ずつ読み込む
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            string[] parts = line.Split(',');
            int table = int.Parse(parts[1]);

            int level = int.Parse(parts[0]);
            _statusData.Add(level, table);
        }
    }

    private void SetDescription() 
    {
        StringReader sr = new StringReader(_descriptionData.text);
        //最初の一行目はスキップ
        sr.ReadLine();

        while (true)
        {
            //一行ずつ読み込む
            string line = sr.ReadLine();

            if (string.IsNullOrEmpty(line))
            {
                break;
            }

            _enhanceData.Add(new EnhanceData(0, line));
        }
    }

    /// <summary>
    /// レベルに対しての次のレベルアップまでの経験値を返してくれる
    /// </summary>
    /// <param name="Level"></param>
    /// <returns></returns>
    public static int GetMaxExperienceAmount(int Level) 
    {
        return _statusData[Level];
    }

}

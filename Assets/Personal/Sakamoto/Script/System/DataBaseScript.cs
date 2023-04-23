using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class DataBaseScript : MonoBehaviour
{
    public static List<EnhanceData> DescriptionEnhanceData => _enhanceData;
    public static Dictionary<WeaponType, string> WeaponDescriptionData => _weaponDescription;

    [Header("レベルアップデータ")]
    [SerializeField] private TextAsset _levelUpTable;
    [SerializeField] private TextAsset _descriptionData;
    [SerializeField] private TextAsset _weaponDescriptionData;
    private static Dictionary<WeaponType, string> _weaponDescription = new Dictionary<WeaponType, string>();
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

    private void SetWeaponTypeDescription()
    {
        //テキストの読み込み
        StringReader sr = new StringReader(_weaponDescriptionData.text);
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
            var weaponType = Enum.Parse<WeaponType>(parts[0]);
            string description = "";

            for (int i = 1; i < parts.Length; i++)
            {
                if (i != 1)
                {
                    description += "\n";
                }

                description += parts[i];
            }
            _weaponDescription.Add(weaponType, description);
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

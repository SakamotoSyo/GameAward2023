using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager
{
    static SaveManager Instance = new SaveManager();

    public static List<string> _weaponFileList = new List<string>();

#if RELEASE
    const string TAIKENFILEPATH = "Taiken.dat";
    const string SOUKENFILEPATH = "Souken.dat";
    const string HAMMERFILEPATH = "Hammer.dat";
    const string YARIFILEPATH = "Yari.dat";
#else
    public const string GREATSWORDFILEPATH = "Taiken.json";
    public const string DUALBLADESFILEPATH = "Souken.json";
    public const string HAMMERFILEPATH = "Hammer.json";
    public const string SPEARFILEPATH = "Yari.json";
#endif
    private SaveData Data;

    public static void Initialize()
    {
        _weaponFileList = new List<string>() { GREATSWORDFILEPATH, DUALBLADESFILEPATH, HAMMERFILEPATH, SPEARFILEPATH };
    }
    static public SaveData Load(string filePath)
    {
        Instance.Data = LocalData.Load<SaveData>(filePath);

        if (Instance.Data == null)
        {
            Instance.Data = new SaveData();
        }
        return Instance.Data;
    }

    static public SaveData GetData(string filePath)
    {
        if (Instance.Data == null)
        {
            Load(filePath);
        }
        return Instance.Data;
    }

    static public void Save(string filePath, SaveData data)
    {
        Instance.Data = data;
        LocalData.Save<SaveData>(filePath, Instance.Data);
    }

    static public void ResetSaveData(string filePath)
    {
        LocalData.ResetSaveData(filePath);
    }
}
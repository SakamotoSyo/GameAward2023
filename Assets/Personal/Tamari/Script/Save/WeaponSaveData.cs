using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSaveData
{
    private static SaveData _gsData = default;
    private static SaveData _dbData = default;
    private static SaveData _hData = default;
    private static SaveData _sData = default;

    private static SaveData _gsSampleData = default;
    private static SaveData _dbSampleData = default;
    private static SaveData _hSampleData = default;
    private static SaveData _sSampleData = default;

    public static SaveData GSData => _gsData;
    public static SaveData DBData => _dbData;
    public static SaveData HData => _hData; 
    public static SaveData SData => _sData;
    public static SaveData GSSampleData => _gsSampleData;
    public static SaveData DBSampleData => _dbSampleData;
    public static SaveData HSampleData => _hSampleData;
    public static SaveData SSampleData => _sSampleData;


    public WeaponSaveData()
    {
        _gsData = SaveManager.Load(SaveManager.GREATSWORDFILEPATH);
        _dbData = SaveManager.Load(SaveManager.DUALBLADESFILEPATH);
        _hData = SaveManager.Load(SaveManager.HAMMERFILEPATH);
        _sData = SaveManager.Load(SaveManager.SPEARFILEPATH);

        _gsSampleData = SaveManager.Load(SaveManager.GREATSWORDSAMPLEFILEPATH);
        _dbSampleData = SaveManager.Load(SaveManager.DUALBLADESSAMPLEFILEPATH);
        _hSampleData = SaveManager.Load(SaveManager.HAMMERSAMPLEFILEPATH);
        _sSampleData = SaveManager.Load(SaveManager.SPEARSAMPLEFILEPATH);
    }
}

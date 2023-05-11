using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSaveData
{
    private static SaveData _gsData = default;
    private static SaveData _dbData = default;
    private static SaveData _hData = default;
    private static SaveData _sData = default;

    public static SaveData GSData => _gsData;
    public static SaveData DBData => _dbData;
    public static SaveData HData => _hData; 
    public static SaveData SData => _sData;

    public WeaponSaveData()
    {
        _gsData = SaveManager.Load(SaveManager.GREATSWORDFILEPATH);
        _dbData = SaveManager.Load(SaveManager.DUALBLADES);
        _hData = SaveManager.Load(SaveManager.HAMMERFILEPATH);
        _sData = SaveManager.Load(SaveManager.SPEARFILEPATH);
    }
}

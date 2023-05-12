using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSaveData
{
    private static SaveData _gsData = default;
    private static SaveData _dbData = default;
    private static SaveData _hData = default;
    private static SaveData _sData = default;

    public static SaveData GSData { get; set; }
    public static SaveData DBData { get; set; }
    public static SaveData HData { get; set; }
    public static SaveData SData { get; set; }

    public WeaponSaveData()
    {
        GSData = SaveManager.Load(SaveManager.GREATSWORDFILEPATH);
        DBData = SaveManager.Load(SaveManager.DUALBLADES);
        HData = SaveManager.Load(SaveManager.HAMMERFILEPATH);
        SData = SaveManager.Load(SaveManager.SPEARFILEPATH);
    }
}

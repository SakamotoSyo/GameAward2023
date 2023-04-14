using UniRx;

// 日本語対応
public struct WeaponData
{
    public float OffensivePower;
    public float WeaponWeight;
    public float CriticalRate;
    public float MaxDurable;
    public float CurrentDurable;
    public float[] StatusArray;
    public AttributeType Attribute;
    public WeaponType WeaponType;


    /// <summary>武器のパラメータを格納する構造体 | 属性は初期設定だと無属性</summary>
    public WeaponData(float offensivePower, float weaponWeight,
        float criticalRate, float durableValue, AttributeType attributeType = AttributeType.None, WeaponType weponType = WeaponType.GreatSword)
    {
        OffensivePower = offensivePower;
        WeaponWeight = weaponWeight;
        CriticalRate = criticalRate;
        MaxDurable = durableValue;
        CurrentDurable = durableValue;
        Attribute = attributeType;
        WeaponType = weponType;

        StatusArray = new float[4];
        StatusArray[0] = offensivePower;
        StatusArray[1] = weaponWeight;
        StatusArray[2] = criticalRate;
        StatusArray[3] = durableValue;
    }

    public void UpdateParam(PlayerEquipWeapon equipWeaponData) 
    {
        OffensivePower = equipWeaponData.OffensivePower.Value;
        WeaponWeight = equipWeaponData.WeaponWeight.Value;
        CriticalRate = equipWeaponData.CriticalRate.Value;
        MaxDurable = equipWeaponData.MaxDurable.Value;
        CurrentDurable = equipWeaponData.CurrentDurable.Value;
        Attribute = equipWeaponData.Attribute;
        WeaponType = equipWeaponData.WeaponType;
    }

    /// <summary>
    /// Statusを配列にして返す
    /// </summary>
    /// <returns></returns>
    public float[] GetStatusArray() 
    {
        float[] statusArray = new float[4];
        statusArray[0] = OffensivePower;
        statusArray[1] = WeaponWeight;
        statusArray[2] = CriticalRate;
        statusArray[3] = MaxDurable;
        return statusArray;
    }
    
    public enum AttributeType
    {
        None,
        Fire,
        Water,
        leaf,
    }
}
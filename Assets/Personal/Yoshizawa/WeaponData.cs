using UniRx;

// 日本語対応
public class WeaponData
{
    public float OffensivePower;
    public float WeaponWeight;
    public float CriticalRate;
    public float MaxDurable;
    public float CurrentDurable;
    public AttributeType Attribute;
    public WeaponType WeaponType;
    

    /// <summary>武器のパラメータを格納する構造体 | 属性は初期設定だと無属性</summary>
    //public WeaponData(float offensivePower, float weaponWeight,
    //    float criticalRate, float durableValue, AttributeType attributeType = AttributeType.None, WeaponType weponType = WeaponType.GreatSword)
    //{
    //    OffensivePower = offensivePower;
    //    WeaponWeight = weaponWeight;
    //    CriticalRate = criticalRate;
    //    MaxDurable = durableValue;
    //    CurrentDurable = durableValue;
    //    Attribute = attributeType;
    //    WeaponType = weponType;
    //}

    public void UpdategeParam(PlayerEquipWeapon equipWeaponData) 
    {
        OffensivePower = equipWeaponData.OffensivePower.Value;
        WeaponWeight = equipWeaponData.WeaponWeight.Value;
        CriticalRate = equipWeaponData.CriticalRate.Value;
        MaxDurable = equipWeaponData.MaxDurable.Value;
        CurrentDurable = equipWeaponData.CurrentDurable.Value;
        Attribute = equipWeaponData.Attribute;
        WeaponType = equipWeaponData.weaponType;
    }
    
    public enum AttributeType
    {
        None,
        Fire,
        Water,
        leaf,
    }
}
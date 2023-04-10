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
    public WeponType WeponType;
    

    /// <summary>武器のパラメータを格納する構造体 | 属性は初期設定だと無属性</summary>
    //public WeaponData(float offensivePower, float weaponWeight,
    //    float criticalRate, float durableValue, AttributeType attributeType = AttributeType.None, WeponType weponType = WeponType.GreatSword)
    //{
    //    OffensivePower = offensivePower;
    //    WeaponWeight = weaponWeight;
    //    CriticalRate = criticalRate;
    //    MaxDurable = durableValue;
    //    CurrentDurable = durableValue;
    //    Attribute = attributeType;
    //    WeponType = weponType;
    //}

    public void UpdategeParam(PlayerEquipWeapon equipWeaponData) 
    {
        OffensivePower = equipWeaponData.OffensivePower.Value;
        WeaponWeight = equipWeaponData.WeaponWeight.Value;
        CriticalRate = equipWeaponData.CriticalRate.Value;
        MaxDurable = equipWeaponData.MaxDurable.Value;
        CurrentDurable = equipWeaponData.CurrentDurable.Value;
        Attribute = equipWeaponData.Attribute;
        WeponType = equipWeaponData.WeponType;
    }
    
    public enum AttributeType
    {
        None,
        Fire,
        Water,
        leaf,
    }
}
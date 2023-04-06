// 日本語対応
public struct WeaponData
{
    public float OffensivePower;
    public float WeaponWeight;
    public float CriticalRate;
    public float DurableValue;
    public AttributeType Attribute;

    /// <summary>武器のパラメータを格納する構造体 | 属性は初期設定だと無属性</summary>
    public WeaponData(float offensivePower, float weaponWeight,
        float criticalRate, float durableValue, AttributeType attributeType = AttributeType.None)
    {
        OffensivePower = offensivePower;
        WeaponWeight = weaponWeight;
        CriticalRate = criticalRate;
        DurableValue = durableValue;
        Attribute = attributeType;
    }

    public enum AttributeType
    {
        None,
        Fire,
        Water,
        leaf,
    }
}
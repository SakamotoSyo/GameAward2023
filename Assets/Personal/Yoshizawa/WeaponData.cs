// 日本語対応
public struct WeaponData
{
    public float OffensivePower;
    public float WeaponWeight;
    public float CriticalRate;
    public float DurableValue;
    public AttributeType Attribute;

    public WeaponData(float power, float weight, float rate, float value, AttributeType type = AttributeType.None)
    {
        OffensivePower = power;
        WeaponWeight = weight;
        CriticalRate = rate;
        DurableValue = value;
        Attribute = type;
    }

    public enum AttributeType
    {
        None,
        Fire,
        Water,
        leaf,
    }
}
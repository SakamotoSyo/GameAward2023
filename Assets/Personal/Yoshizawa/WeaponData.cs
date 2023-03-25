// 日本語対応
public struct WeaponData
{
    public float OffensivePower;
    public float WeaponWeight;
    public float CriticalRate;
    public AttributeType Attribute;

    public WeaponData(float power, float weight, float rate, AttributeType type)
    {
        OffensivePower = power;
        WeaponWeight = weight;
        CriticalRate = rate;
        Attribute = type;
    }

    public enum AttributeType
    {
        None,
        Fire,
        Water,
        Wood,
    }
}
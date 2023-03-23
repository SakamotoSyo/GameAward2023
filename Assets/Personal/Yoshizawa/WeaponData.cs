using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 日本語対応
public class WeaponData
{
    public WeaponData()
    {
        
    }

    public enum AttributeType
    {
        None,
        Fire,
        Water,
        Wood,
    }
}

struct WeaponParameter
{
    public float OffensivePower;
    public float WeaponWeight;
    public float CriticalRate;

    public WeaponParameter(float power, float weight, float rate)
    {
        OffensivePower = power;
        WeaponWeight = weight;
        CriticalRate = rate;
    }
}
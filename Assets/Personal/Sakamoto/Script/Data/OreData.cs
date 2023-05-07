using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct OreData
{
    public EnhanceData[] EnhancedData;
    public SkillBase Skill;
    public Sprite OreImage;
    public OreRarity Rarity;

    public OreData(EnhanceData[] enhanceDatas,OreRarity oreRarity, SkillBase skill, Sprite image) 
    {
        EnhancedData = enhanceDatas;
        Rarity = oreRarity;
        Skill = skill;
        OreImage = image;
    }
}

public enum OreRarity 
{
    Normal,
    Rare,
    Epic,
}
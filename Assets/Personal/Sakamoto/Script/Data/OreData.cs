using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct OreData
{
    public EnhanceData[] EnhancedData;
    public ISkillBase Skill;
    public Image OreImage;
    public OreRarity Rarity;

    public OreData(EnhanceData[] enhanceDatas,OreRarity oreRarity, ISkillBase skill, Image image) 
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
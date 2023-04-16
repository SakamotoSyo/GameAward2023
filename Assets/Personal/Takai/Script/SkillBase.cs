using UnityEngine;
using Cysharp.Threading.Tasks;

public abstract class SkillBase : MonoBehaviour
{
    public string SkillName { get; set; }
    public int Damage { get; set; }
    public WeaponType Weapon { get; set; }
    public OreRarity Rarity { get; set; }
    public SkillType Type { get; set; }

    public abstract UniTask UseSkill();

    protected virtual void SkillEffect()
    {
        
    }
}

public enum SkillType
{
    Skill,
    Special,
}
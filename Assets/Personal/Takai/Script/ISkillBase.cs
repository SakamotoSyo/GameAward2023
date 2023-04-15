using Cysharp.Threading.Tasks;
public interface ISkillBase
{
    string SkillName { get; set; }
    int Damage { get; set; }
    WeaponType Weapon { get; set; }
    OreRarity Rarity { get; set; }
    SkillType Type { get; set; }
    PlayerSkillDataManagement playerSkillDataManagement { get; set; }
    
    void SetUp();
    void SendSkill();
    UniTask UseSkill();
    void SkillEffect();
}

public enum SkillType
{
    Skill,
    Special,
}
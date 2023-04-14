public interface ISkillBase
{
    string SkillName { get; set; }
    int Damage { get; set; }
    WeaponType Type { get; set; }
    OreRarity Rarity { get; set; }
    PlayerSkillDataManagement playerSkillDataManagement { get; set; }
    
    void SetUp();
    void SendSkill();
    void UseSkill();
    void SkillEffect();
}
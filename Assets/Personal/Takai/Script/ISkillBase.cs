public interface ISkillBase
{
    int SkillId { get; set; }
    string SkillName { get; set; }
    int RequiredSP { get; set; }
    int Damage { get; set; }
    WeponType Type { get; set; }

    void SetUp();
    void SendSkill();
    void UseSkill();
    void SkillEffect();
}

public enum WeponType
{
    LargeSword,
    TwoSword,
    Hammer,
    Spear,
}
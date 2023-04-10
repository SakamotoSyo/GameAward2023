public interface ISkillBase
{
    int SkillId { get; set; }
    string SkillName { get; set; }
    int RequiredSP { get; set; }
    int Damage { get; set; }
    WeaponType Type { get; set; }
    SkillAcquisition SkillAcquisition { get; set; }
    
    void SetUp();
    void SendSkill();
    void UseSkill();
    void SkillEffect();
}
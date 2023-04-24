using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public SkillBase SpecialAttack => _specialAttack;
    public SkillBase[] PlayerSkillArray => _skillArray;
    [Tooltip("•KŽE‹Z")]
    private SkillBase _specialAttack;
    private SkillBase[] _skillArray = new SkillBase[2];

    public void SaveSkill(PlayerSaveData saveData) 
    {
        saveData.SpecialAttack = _specialAttack;
        saveData.PlayerSkillArray = _skillArray;
    }

}

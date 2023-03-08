using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSkill : MonoBehaviour
{
    [SerializeField] private SkillName _skillName;
    [SerializeField] private SkillDataBase _skillData;

    public async void Start()
    {
        ISkill skill = _skillData.SkillList.Find(x => x.SkillName == _skillName)
               .SkillObj.GetComponent<ISkill>();

        await skill.StartSkill();
        Debug.Log(skill.SkillResult());
        skill.SkillEnd();
    }
}

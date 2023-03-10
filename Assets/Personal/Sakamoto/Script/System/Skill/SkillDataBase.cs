using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataBase : MonoBehaviour
{
    public List<SkillListData> SkillList => _skillList;
    [SerializeField] private List<SkillListData> _skillList;

    [System.Serializable]
    public class SkillListData
    {
        public GameObject SkillObj;
        public SkillName SkillName;
    }
}

public enum SkillName
{
    Timing,
    Gauge,

}

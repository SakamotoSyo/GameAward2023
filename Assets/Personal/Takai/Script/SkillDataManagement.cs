using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillDataManagement : MonoBehaviour
{
    [Header("スキル検索"), SerializeField] private string _skillName;

    [SerializeField] private ActorGenerator _actorGenerator;
    [SerializeField] private PlayerController _pStatus;
    [SerializeField] private EnemyController _eStatus;

    private List<SkillBase> _skills = new List<SkillBase>();
    private List<SkillBase> _skillUsePool = new List<SkillBase>();
    public IReadOnlyList<SkillBase> PlayerSkillList => _skills;

    private void Awake()
    {
        SkillBase[] skillPrefabs = Resources.LoadAll<SkillBase>("Skills");

        foreach (var skill in skillPrefabs)
        {
            Instantiate(skill, transform);
            _skills.Add(skill);
        }
    }

    public SkillBase OnSkillCall(WeaponType weapon, SkillType type)
    {
        List<SkillBase> skills = new List<SkillBase>();

        foreach (var s in _skills)
        {
            if (s.Weapon == weapon && s.Type == type)
            {
                skills.Add(s);
            }
        }

        int n = Random.Range(0, skills.Count);

        return skills[n];
    }

    public bool OnUseCheck(SkillBase skill)
    {
        return skill.IsUseCheck(_actorGenerator.PlayerController);
    }

    public void OnSkillUse(ActorAttackType actorType, string skillName)
    {
        SkillBase skill = _skills.Find(skill => skill.name == skillName);
        if (skill != null)
        {
            
            _pStatus = _actorGenerator.PlayerController;
            _eStatus = _actorGenerator.EnemyController;
            Debug.Log(skill.name);
            skill.UseSkill(_pStatus, _eStatus, actorType);
        }
        else
        {
            Debug.Log("スキルが見つかりません");
        }
    }

    public bool CounterCheck()
    {
        //_skillUsePoolにカウンターがあるか調べてSkillの関数を発動する
        return default;
    }

    public void TurnCall()
    {
        Debug.Log("TurnCall呼び出し");
        foreach (var skill in _skills)
        {
            if (skill.gameObject.activeSelf)
            {
                bool IsUse = skill.TurnEnd();
                if (IsUse)
                {
                    _skillUsePool.Add(skill);
                }
            }
        }
    }

    public void CallBattleFinish()
    {
        Debug.Log("BattleFinish呼び出し");
        foreach (var skill in _skills)
        {
            if (skill.gameObject.activeSelf)
            {
                skill.BattleFinish();
            }
        } 
    }

    public SkillBase SearchSkill()
    {
        foreach (var s in _skills)
        {
            if (s.SkillName == _skillName)
            {
                return s;
            }
        }

        Debug.Log("スキル名が一致しません");
        return null;
    }
}

public enum ActorAttackType
{
    Player,
    Enemy,
}
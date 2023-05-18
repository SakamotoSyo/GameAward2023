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
    [SerializeField] private Transform _transform;
    [SerializeField] private PlayerController _pStatus;
    [SerializeField] private EnemyController _eStatus;
    [SerializeField] private List<GameObject> _skillPrefab = new();

    private List<SkillBase> _skills = new List<SkillBase>();
    private List<SkillBase> _skillUsePool = new List<SkillBase>();
    public IReadOnlyList<SkillBase> PlayerSkillList => _skills;

    private void Awake()
    {
       // SkillBase[] skillPrefabs = Resources.LoadAll<SkillBase>("Skills");

        for (int i = 0; i < _skillPrefab.Count; i++) 
        {
            var skillObj = Instantiate(_skillPrefab[i], _transform);
            _skills.Add(skillObj.GetComponent<SkillBase>());
        }

        //foreach (var skill in skillPrefabs)
        //{
        //    Instantiate(skill, transform);
        //    _skills.Add(skill);
        //}
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

    public async UniTask OnSkillUse(ActorAttackType actorType, string skillName)
    {
        SkillBase skill = _skills.Find(skill => skill.name == skillName);
        if (skill != null)
        {
            _pStatus = _actorGenerator.PlayerController;
            _eStatus = _actorGenerator.EnemyController;
            Debug.Log(skill.name);
            await skill.UseSkill(_pStatus, _eStatus, actorType);
            _skillUsePool.Add(skill);
        }
        else
        {
            Debug.LogError("スキルが見つかりません");
        }
    }

    public async UniTask<bool> InEffectCheck(string skillName, ActorAttackType attackType)
    {
        foreach (var s in _skillUsePool)
        {
            if (s.SkillName == skillName)
            {
                await s.InEffectSkill(attackType);
                return true;
            }
        }

        return false;
        //_skillUsePoolにカウンターがあるか調べてSkillの関数を発動する
    }

    public void TurnCall()
    {
        Debug.Log("TurnCall呼び出し");
        foreach (var skill in _skillUsePool)
        {
            if (skill.gameObject.activeSelf)
            {
                bool IsUse = skill.TurnEnd();
                if (!IsUse)
                {
                    _skillUsePool.Remove(skill);
                }
            }
        }
    }

    public void CallBattleFinish()
    {
        Debug.Log("BattleFinish呼び出し");
        foreach (var skill in _skillUsePool)
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

        if (_skillName != "")
        {
            Debug.LogError("スキル名が一致しません");
        }
        return null;
    }
}

public enum ActorAttackType
{
    Player,
    Enemy,
}
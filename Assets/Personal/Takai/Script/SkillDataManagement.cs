using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SkillDataManagement : MonoBehaviour
{
    [Header("スキル検索"), SerializeField] private string _skillName;
    [SerializeField] private Transform _transform;
    [SerializeField] private PlayerController _pStatus;
    [SerializeField] private EnemyController _eStatus;
    [SerializeField] private List<GameObject> _skillPrefab = new();
    [SerializeField] private Transform _playerVec;

    private ActorGenerator _actorGenerator;
    private static List<SkillBase> _skills = new List<SkillBase>();
    private List<SkillBase> _skillUsePool = new List<SkillBase>();
    private static SkillDataManagement _skill;
    public IReadOnlyList<SkillBase> PlayerSkillList => _skills;

    private void Awake()
    {
        // SkillBase[] skillPrefabs = Resources.LoadAll<SkillBase>("Skills");
        var skillIns = FindObjectOfType<SkillDataManagement>();
        if (_skill == null) 
        {
            DontDestroyOnLoad(this.gameObject);
            _skill = this;
        }
        else if(skillIns != _skill)
        {
            Destroy(this);
        }

        for (int i = 0; i < _skillPrefab.Count; i++) 
        {
            var skillObj = Instantiate(_skillPrefab[i], this.transform);
            _skills.Add(skillObj.GetComponent<SkillBase>());
            skillObj.transform.position = new Vector2(_playerVec.position.x + 2.5f,_playerVec.position.y - 1.5f);
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
        if (!_actorGenerator) 
        {
            _actorGenerator = GameObject.Find("ActorGenerator").GetComponent<ActorGenerator>();
        }
        Debug.Log(skillName);
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
        List<SkillBase> skillsToRemove = new List<SkillBase>();
        foreach (var skill in _skillUsePool)
        {
            if (skill.gameObject.activeSelf)
            {
                bool IsUse = skill.TurnEnd();
                if (!IsUse)
                {
                    skillsToRemove.Add(skill);
                }
            }
        }

        foreach (var skillToRemove in skillsToRemove)
        {
            _skillUsePool.Remove(skillToRemove);
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
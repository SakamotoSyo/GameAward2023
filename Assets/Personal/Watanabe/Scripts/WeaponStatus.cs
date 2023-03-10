using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    public ISkill Skill => _skill;

    [SerializeField] private AttackType _type = AttackType.Normal;
    [SerializeField] private SkillName _skillName;
    [SerializeField] private SkillDataBase _skillData;
    [SerializeField] private Animator _playerAnim;
    [SerializeField] private Animator _playerEffectAnim;
    [SerializeField] private Animator _enemyEffectAnim;

    [Header("武器のステータス一覧")]
    [SerializeField] private List<int> _values = new();
    [Tooltip("上昇値")]
    [SerializeField] private int _updateValue = 1;
    [Tooltip("会心の確率(5～50)")]
    [Range(5, 50)]
    [SerializeField] private int _probCritical = 10;

    private ISkill _skill;
    private readonly List<int> _defaultValue = new();
    private readonly bool[] _updating = new bool[] { false, false, false };

    bool _isAttack = false; //高井が書き加えました

    //以下UI表示用
    public int AttackValue
    {
        get => _values[0];
        protected set => _values[0] = value;
    }
    public int TestValue
    {
        get => _values[1];
        protected set => _values[1] = value;
    }

    private void Start()
    {
        for (int i = 0; i < _values.Count; i++)
        {
            _defaultValue.Add(0);
            _defaultValue[i] = _values[i];
        }
    }

    private async void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || _isAttack)
        {
            ResetValues();

            if (_type == AttackType.Normal)
            {
                _isAttack = false;

                if (FindObjectOfType<EnemyController>().TryGetComponent<IAddDamage>(out IAddDamage enemy))
                {
                    enemy.AddDamage(_values[0]);
                }
            }
            else if (_type == AttackType.Skill)
            {
                _isAttack = false;

                _skill = _skillData.SkillList.Find(x => x.SkillName == _skillName)
                       .SkillObj.GetComponent<ISkill>();

                AnimBool(true);
                await _skill.StartSkill();
                AnimBool(false);
                _values[0] += (int)(_updateValue * _skill.SkillResult());
                //PlayUpdate(skill.SkillResult());

                if (FindObjectOfType<EnemyController>().TryGetComponent<IAddDamage>(out IAddDamage enemy))
                {
                    enemy.AddDamage(_values[0]);
                }
                _skill.SkillEnd();
            }
        }
    }

    private void AnimBool(bool setActive)
    {
        _playerEffectAnim.SetBool(_skill.SkillEffectAnimName(), setActive);
        _enemyEffectAnim.SetBool(_skill.SkillEffectAnimName(), !setActive);
    }

    private void ResetValues()
    {
        for (int i = 0; i < _values.Count; i++)
        {
            _values[i] = _defaultValue[i];
        }
    }

    // 高井が書き加えました-------
    public void OnCallAttack(int type)
    {
        if (type < 0 || type > Enum.GetValues(typeof(AttackType)).Length) { return; }
        _type = (AttackType)Enum.ToObject(typeof(AttackType), type);
        _isAttack = true;
    }
    //----------------------------
}

public enum AttackType
{
    Normal,
    Skill,
}

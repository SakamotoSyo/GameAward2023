using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    public ISkill Skill => _skill;

    [SerializeField] private AttackType _type = AttackType.Normal;
    [SerializeField] private SkillName _skillName;
    [SerializeField] private SkillDataBase _skillData;
    [SerializeField] private Animator _playerAnim;

    [Header("武器のステータス一覧")]
    [SerializeField] private List<int> _values = new();
    [Tooltip("上昇値")]
    [SerializeField] private int _updateValue = 1;
    [Tooltip("会心の確率(5～50)")]
    [Range(5, 50)]
    [SerializeField] private int _probCritical = 10;

    private ISkill _skill;
    private readonly List<int> _defaultValue = new();
    private readonly bool[] _updating = new bool[] {false, false, false};

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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ResetValues();

            if (_type == AttackType.Normal)
            {
                if (FindObjectOfType<EnemyController>().TryGetComponent<IAddDamage>(out IAddDamage enemy))
                {
                    enemy.AddDamage(_values[0]);
                }
            }
            else if (_type == AttackType.Skill)
            {
                _skill = _skillData.SkillList.Find(x => x.SkillName == _skillName)
                       .SkillObj.GetComponent<ISkill>();

                _playerAnim.SetBool(_skill.SkillEffectPlayerAnim(), true);
                await _skill.StartSkill();
                _playerAnim.SetBool(_skill.SkillEffectPlayerAnim(), false);
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

    private void ResetValues()
    {
        for (int i = 0; i < _values.Count; i++)
        {
            _values[i] = _defaultValue[i];
        }
    }
}

public enum AttackType
{
    Normal,
    Skill,
}

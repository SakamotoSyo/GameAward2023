using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class WeaponStatus : MonoBehaviour
{
    public ISkill Skill => _skill;

    [SerializeField] private AttackType _type = AttackType.Normal;
    [SerializeField] private SkillName _skillName;
    [SerializeField] private SkillDataBase _skillData;
    [SerializeField] private Animator _playerAnim;
    [SerializeField] private Animator _playerEffectAnim;
    [SerializeField] private Animator _enemyEffectAnim;
    [SerializeField] private bool _isClick = false;
    [SerializeField] private bool _isMove = false;
    [SerializeField] private UnityEvent _onCritical;

    [Header("武器のステータス一覧")]
    [SerializeField] private List<int> _values = new();
    [Tooltip("上昇値")]
    [SerializeField] private int _updateValue = 1;
    [Tooltip("会心の確率(5～50)")]
    [Range(5, 50)]
    [SerializeField] private int _probCritical = 10;
    [Tooltip("会心の一撃が出たときにかかる攻撃力の倍率")]
    [SerializeField] private float _criticalMultiplier;
    [SerializeField] private AudioClip[] _clip;//大橋が書き加えました
    private AudioSource _source;//大橋が書き加えました
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
        _source = GetComponent<AudioSource>();//大橋
        for (int i = 0; i < _values.Count; i++)
        {
            _defaultValue.Add(0);
            _defaultValue[i] = _values[i];
        }
    }

    private async void Update()
    {
        if (_isClick)
        {
            //MouseMock用
            ClickAction();
        }
        else
        {
            ButtonSelectAction();
        }
    }

    public async void ClickAction()
    {
        if (Input.GetMouseButtonDown(0) && !_isAttack)
        {
            ResetValues();
            _isAttack = true;
            EnemyDamage();
            _isAttack = false;
        }
        else if (Input.GetMouseButtonDown(1) && !_isAttack)
        {
            ResetValues();
            _isAttack = true;

            _skill = _skillData.SkillList.Find(x => x.SkillName == _skillName)
                   .SkillObj.GetComponent<ISkill>();

            AnimBool(true);
            await _skill.StartSkill();
            AnimBool(false);
            _values[0] += (int)(_updateValue * _skill.SkillResult());
            EnemyDamage();
            _skill.SkillEnd();
            _isAttack = false;
        }

    }

    public async void ButtonSelectAction() 
    {
        if (Input.GetKeyDown(KeyCode.Return) || _isAttack)
        {
            ResetValues();
            if (_type == AttackType.Normal)
            {
                _isAttack = false;
                EnemyDamage();
            }
            else if (_type == AttackType.Skill)
            {
                _isAttack = false;
                _skill = _skillData.SkillList.Find(x => x.SkillName == _skillName)
                       .SkillObj.GetComponent<ISkill>();

                AnimBool(true);
                await _skill.StartSkill();
                AnimBool(false);
                _values[0] = (int)(_values[0] * _skill.SkillResult());
                EnemyDamage();
                _skill.SkillEnd();
            }
           
        }
    }


    private void EnemyDamage() 
    {
        //会心の一撃
        var num = UnityEngine.Random.Range(1, 101);
        if (num <= _probCritical)
        {
            _values[0] = (int)(_values[0] * _criticalMultiplier);
            _source.PlayOneShot(_clip[(int)AttackType.Critical]);
        }
        else 
        {
            _source.PlayOneShot(_clip[(int)_type]);
        }

        if (FindObjectOfType<EnemyController>().TryGetComponent<IAddDamage>(out IAddDamage enemy))
        {
            enemy.AddDamage(_values[0]);
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
    Critical,
}

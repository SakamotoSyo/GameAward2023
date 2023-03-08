using System.Collections.Generic;
using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    [SerializeField] private SkillName _skillName;
    [SerializeField] private SkillDataBase _skillData;

    [Header("武器のステータス一覧")]
    [SerializeField] private List<float> _values = new();
    [Tooltip("上昇値")]
    [SerializeField] private float _updateValue = 1f;
    [Tooltip("会心の確率(5～50)")]
    [Range(5, 50)]
    [SerializeField] private int _probCritical = 10;

    private bool[] _updating = new bool[] {false, false, false};

    //以下UI表示用
    public float AttackValue
    { 
        get => _values[0];
        protected set => _values[0] = value;
    }
    public float CriticalValue
    {
        get => _values[1];
        protected set => _values[1] = value;
    }
    public float TestValue
    {
        get => _values[2];
        protected set => _values[2] = value;
    }

    private async void Start()
    {
        ISkill skill = _skillData.SkillList.Find(x => x.SkillName == _skillName)
               .SkillObj.GetComponent<ISkill>();

        await skill.StartSkill();
        PlayUpdate(skill.SkillResult());
        skill.SkillEnd();
    }

    public void SwitchValue(int index)
    {
        _updating[index] = true;
        Debug.Log($"{index + 1}番目の値を更新します");
    }

    private void PlayUpdate(float value)
    {
        for (int i = 0; i < _updating.Length; i++)
        {
            if (_updating[i])
                _values[i] += _updateValue * value;

            _updating[i] = false;
        }
        Debug.Log($"武器の指定したステータスが{value}倍になりました");
    }
}

using UnityEngine;

public class WeaponStatus : MonoBehaviour
{
    [Header("武器のステータス一覧")]
    [Tooltip("上から、攻撃力、会心、Test")]
    [SerializeField] private float[] _values = new float[3];
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
    public int ProbCritical
    {
        get => _probCritical;
        protected set => _probCritical = value;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void SwitchValue(int index)
    {
        _updating[index] = true;
        Debug.Log($"{index + 1}番目の値を更新します");
    }

    public void PlayUpdate()
    {
        for (int i = 0; i < _updating.Length; i++)
        {
            if (_updating[i])
                _values[i] += _updateValue;

            _updating[i] = false;
        }
        Debug.Log("武器のステータスが更新されました");
    }
}

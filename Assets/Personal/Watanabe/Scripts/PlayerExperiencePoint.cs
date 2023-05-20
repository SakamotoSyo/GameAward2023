using UnityEngine;

public class PlayerExperiencePoint : MonoBehaviour
{
    [Tooltip("Playerの経験値")]
    [SerializeField] private int _experiencePoint = 0;
    [Tooltip("Cランクの最大値")]
    [SerializeField] private int _rankCMaxValue = 100;
    [Tooltip("Bランクの最大値")]
    [SerializeField] private int _rankBMaxValue = 200;
    [Tooltip("Aランクの最大値")]
    [SerializeField] private int _rankAMaxValue = 300;

    private const int RANK_C = 0;
    private const int RANK_B = 1;
    private const int RANK_A = 2;
    private const int RANK_S = 3;

    public int RankCMaxValue => _rankCMaxValue;
    public int RankBMaxValue => _rankBMaxValue;
    public int RankAMaxValue => _rankAMaxValue;
    public int ExperiencePoint { get => _experiencePoint; set => _experiencePoint = value; }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public int RankSetting()
    {
        if (_experiencePoint < _rankCMaxValue)
        {
            return RANK_C;
        }
        else if (_experiencePoint < _rankBMaxValue)
        {
            return RANK_B;
        }
        else if (_experiencePoint < _rankAMaxValue)
        {
            return RANK_A;
        }
        return RANK_S;
    }
}

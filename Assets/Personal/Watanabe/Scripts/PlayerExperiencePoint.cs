using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperiencePoint : MonoBehaviour
{
    #region ランクアップ演出関連
    [Header("ランクアップ演出関連")]
    [SerializeField] private Sprite[] _ranks = default;
    [SerializeField] private GameObject _currentRank = default;
    [Tooltip("拡大値")]
    [SerializeField] private float _scaleValue = 1f;
    [Tooltip("拡大後、縮小するまで待つ秒数")]
    [SerializeField] private float _waitSecondForRank = 1f;

    private RectTransform _rankRect = default;
    private Vector3 _pos = Vector3.zero;
    private Image _image = default;
    private int _index = 0;
    #endregion

    #region ランクアップに必要な数値関連
    [Header("数値関連")]
    [Tooltip("Cランクの最大値")]
    [SerializeField] private int _rankCMaxValue = 2500;
    [Tooltip("Bランクの最大値")]
    [SerializeField] private int _rankBMaxValue = 6000;
    [Tooltip("Aランクの最大値")]
    [SerializeField] private int _rankAMaxValue = 10000;
    [Tooltip("Sランクの最大値")]
    [SerializeField] private int _rankSMaxValue = 15000;
    [SerializeField] private Image _pointValueImage = default;

    /// <summary> バトルにいく前の経験値 </summary>
    private int _beforeBattlePoint = 0;
    /// <summary> Playerの経験値 </summary>
    private static int _experiencePoint = 0;

    private const int RANK_C = 0;
    private const int RANK_B = 1;
    private const int RANK_A = 2;
    private const int RANK_S = 3;

    private static int _currentRankNum = 0;
    private static float _value = 0;

    public static int ExperiencePoint => _experiencePoint;
    public static int CurrentRankNum => _currentRankNum;
    public static float Value => _value;
    #endregion

    private void Awake()
    {
        _index = RankSetting();
        ValueSet(_index);

        _experiencePoint = GameManager.PlayerSaveData.PlayerRankPoint;
        //_experiencePoint = 3000;

        _pointValueImage.fillAmount = _beforeBattlePoint / _value;

        SettingsRankUI();
    }

    /// <summary> RankUI関連の情報を取得 </summary>
    private void SettingsRankUI()
    {
        _rankRect = _currentRank.GetComponent<RectTransform>();
        _pos = _rankRect.localPosition;
        _image = _currentRank.GetComponent<Image>();

        _image.sprite = _ranks[_index];
    }

    private int RankSetting()
    {
        if (_beforeBattlePoint < _rankCMaxValue)
        {
            _currentRankNum = RANK_C;
            return RANK_C;
        }
        else if (_beforeBattlePoint < _rankBMaxValue)
        {
            _currentRankNum = RANK_B;
            return RANK_B;
        }
        else if (_beforeBattlePoint < _rankAMaxValue)
        {
            _currentRankNum = RANK_A;
            return RANK_A;
        }
        _currentRankNum = RANK_S;
        return RANK_S;
    }

    public int RankSetting(bool isBossClear)
    {
        if (_beforeBattlePoint < _rankCMaxValue || (_beforeBattlePoint < _rankBMaxValue && !isBossClear))
        {
            _currentRankNum = RANK_C;
            return RANK_C;
        }
        else if (_beforeBattlePoint < _rankBMaxValue || (_beforeBattlePoint < _rankAMaxValue && !isBossClear))
        {
            _currentRankNum = RANK_B;
            return RANK_B;
        }
        else if (_beforeBattlePoint < _rankAMaxValue || (_beforeBattlePoint < _rankSMaxValue && !isBossClear))
        {
            _currentRankNum = RANK_A;
            return RANK_A;
        }
        _currentRankNum = RANK_S;
        return RANK_S;
    }

    /// <summary> 経験値の反映 </summary>
    public void PointSetting()
    {
        ValueSet(_currentRankNum);

        var sequence = DOTween.Sequence();

        sequence.Append(_pointValueImage.DOFillAmount(_experiencePoint / _value, 1.5f))
                .AppendCallback(() =>
                {
                    _beforeBattlePoint = _experiencePoint;
                })
                .AppendCallback(() =>
                {
                    //経験値がランク上限までいく
                    if (Mathf.Approximately(_pointValueImage.fillAmount, 1f))
                    {
                        //GameManager.IsBossClear = true;

                        //昇格戦クリアしたら
                        if (GameManager.IsBossClear)
                        {
                            Debug.Log("rankup");
                            RankUp();
                            HomeScene.Instance.IsRankUp = false;
                            GameManager.IsBossClear = false;
                        }
                        else
                        {
                            HomeScene.Instance.IsRankUp = true;
                        }
                    }
                });
    }

    /// <summary> ランクアップ演出 </summary>
    private void RankUp()
    {
        if (_index == RANK_S)
        {
            return;
        }

        //最大ランクでなければ
        if (_index != _ranks.Length - 1)
        {
            var sequence = DOTween.Sequence();

            //演出実行
            sequence.Append(_rankRect.DOAnchorPos(new Vector3(0f, 0f, 0f), 0.6f))
                    .AppendInterval(_waitSecondForRank)
                    .AppendCallback(() =>
                    {
                        _index++;
                        _image.sprite = _ranks[_index];
                    })
                    .AppendCallback(() =>
                    {
                        ValueSet(_index);
                        _pointValueImage.fillAmount = _experiencePoint / _value;
                        //ここで音流す↓(ランク上がったぽいの)
                        //SoundManager.Instance.CriAtomPlay(CueSheet.SE, "");
                    })
                    .Join(_currentRank.transform.DOScale(new Vector3(1f, 1f, 1f) * _scaleValue, 0.2f))
                    .AppendInterval(_waitSecondForRank)
                    .Append(_currentRank.transform.DOScale(new Vector3(1f, 1f, 1f), 1.5f))
                    .Join(_rankRect.DOAnchorPos(_pos, 1.5f));
        }
    }

    /// <summary> 自分のランクから、上限値を設定する </summary>
    private void ValueSet(int num)
    {
        _value = num switch
        {
            RANK_C => _rankCMaxValue,
            RANK_B => _rankBMaxValue,
            RANK_A => _rankAMaxValue,
            RANK_S => _rankSMaxValue,
            _ => 1,
        };
    }

    /// <summary> バトルに挑む前の経験値を保存しておく </summary>
    public void SetExperiencePoint()
    {
        _beforeBattlePoint = GameManager.PlayerSaveData.PlayerRankPoint;
    }
}

﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperiencePoint : MonoBehaviour
{
    #region ランクアップ関連
    [Header("ランクアップ関連")]
    [SerializeField] private Sprite[] _ranks = default;
    [SerializeField] private GameObject _currentRank = default;
    [Tooltip("拡大値")]
    [SerializeField] private float _scaleValue = 1f;
    [Tooltip("拡大後、縮小するまで待つ秒数")]
    [SerializeField] private float _waitSecondForRank = 1f;

    [Header("Debug")]
    [Tooltip("ランクが上がったか")]
    [SerializeField] private bool _isRankUp = false;

    private RectTransform _rankRect = default;
    private Vector3 _pos = Vector3.zero;
    private Image _image = default;
    private int _index = 0;
    #endregion

    [Tooltip("バトルにいく前の経験値")]
    [SerializeField] private int _beforeBattlePoint = 0;
    [Tooltip("Playerの経験値")]
    [SerializeField] private int _experiencePoint = 0;
    [Tooltip("Cランクの最大値")]
    [SerializeField] private int _rankCMaxValue = 100;
    [Tooltip("Bランクの最大値")]
    [SerializeField] private int _rankBMaxValue = 200;
    [Tooltip("Aランクの最大値")]
    [SerializeField] private int _rankAMaxValue = 300;
    [Tooltip("Sランクの最大値")]
    [SerializeField] private int _rankSMaxValue = 400;
    [SerializeField] private Image _pointValueImage = default;

    private const int RANK_C = 0;
    private const int RANK_B = 1;
    private const int RANK_A = 2;
    private const int RANK_S = 3;
    private int _currentRankNum = 0;
    private float _value = 0;

    public static PlayerExperiencePoint Instance = default;

    public int BeforeBattlePoint { get => _beforeBattlePoint; set => _beforeBattlePoint = value; }
    public int ExperiencePoint { get => _experiencePoint; set => _experiencePoint = value; }

    private void Start()
    {
        _index = RankSetting();
        ValueSet(_index);
        _pointValueImage.fillAmount = _beforeBattlePoint / _value;

        SettingsRankUI();
        Instance = this;
    }

    private void SettingsRankUI()
    {
        //RankUI関連の情報を取得
        _rankRect = _currentRank.GetComponent<RectTransform>();
        _pos = _rankRect.localPosition;
        _image = _currentRank.GetComponent<Image>();

        _image.sprite = _ranks[_index];
    }

    public int RankSetting()
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
                    //経験値がランク上限までいったら
                    if (Mathf.Approximately(_pointValueImage.fillAmount, 1f))
                    {
                        Debug.Log("rankup");
                        RankUp();
                    }
                });
    }

    /// <summary> ランクアップ演出(フェード、経験値上昇の動きが終わってから呼ぶ) </summary>
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

            //RankUIの一連の動きをDOTweenでやる
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
                    })
                    .Join(_currentRank.transform.DOScale(new Vector3(1f, 1f, 1f) * _scaleValue, 0.2f))
                    .AppendInterval(_waitSecondForRank)
                    .Append(_currentRank.transform.DOScale(new Vector3(1f, 1f, 1f), 1.5f))
                    .Join(_rankRect.DOAnchorPos(_pos, 1.5f));
        }
    }

    private void ValueSet(int num)
    {
        _value = num switch
        {
            RANK_C => _rankCMaxValue,
            RANK_B => _rankCMaxValue + _rankBMaxValue,
            RANK_A => _rankCMaxValue + _rankBMaxValue + _rankAMaxValue,
            RANK_S => _rankCMaxValue + _rankBMaxValue + _rankAMaxValue + _rankSMaxValue,
            _ => 1,
        };
    }
}
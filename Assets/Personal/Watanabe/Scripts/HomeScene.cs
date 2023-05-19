using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
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
    [Tooltip("昇格戦挑戦可か")]
    [SerializeField] private bool _isChallengablePromotionMatch = false;

    private RectTransform _rankRect = default;
    private Vector3 _pos = Vector3.zero;
    private Image _image = default;
    private int _index = 0;
    #endregion

    #region カット演出系
    [SerializeField] private GameObject _cutPanelParent = default;
    [SerializeField] private float _waitSecondCutStaging = 1f;

    private Image[] _cutPanels = new Image[3];
    private Text[] _cutTexts = new Text[2];
    #endregion

    private void Start()
    {
        SettingsRankUI();
        SettingsCutPanel();

        if (_isChallengablePromotionMatch)
        {
            //カットシーン演出
            Fade.Instance.FadeOut();
        }
    }

    private void Update()
    {
        //以下test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ランクアップ演出
            RankUp();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            //カットシーン演出
            Fade.Instance.FadeOut();
        }
    }

    private void SettingsRankUI()
    {
        //RankUI関連の情報を取得
        _rankRect = _currentRank.GetComponent<RectTransform>();
        _pos = _rankRect.localPosition;
        _image = _currentRank.GetComponent<Image>();

        _index = Array.IndexOf(_ranks, _image.sprite);
    }

    private void SettingsCutPanel()
    {
        for (int i = 0; i < _cutPanelParent.transform.childCount; i++)
        {
            if (i == 0 || i == _cutPanelParent.transform.childCount - 1)
            {
                if (i == 0)
                {
                    _cutPanels[i] = _cutPanelParent.transform.GetChild(i).GetComponent<Image>();
                    _cutPanels[i + 1] = _cutPanels[i].transform.GetChild(i).GetComponent<Image>();
                }
                else
                {
                    _cutPanels[i - 1] = _cutPanelParent.transform.GetChild(i).GetComponent<Image>();
                }
            }
            else
            {
                _cutTexts[i - 1] = _cutPanelParent.transform.GetChild(i).GetComponent<Text>();
            }
        }
    }

    /// <summary> ランクアップ演出(フェード、経験値上昇の動きが終わってから呼ぶ) </summary>
    private void RankUp()
    {
        //最大ランクでなければ
        if (_index != _ranks.Length - 1)
        {
            var sequence = DOTween.Sequence();

            //RankUIの一連の動きをDOTweenでやる
            sequence.Append(_rankRect.DOAnchorPos(new Vector3(0f, 0f, 0f), 1f))
                    .AppendInterval(_waitSecondForRank)
                    .AppendCallback(() =>
                    {
                        _index++;
                        _image.sprite = _ranks[_index];
                    })
                    .Append(_currentRank.transform.DOScale(new Vector3(1f, 1f, 1f) * _scaleValue, 0.2f))
                    .AppendInterval(_waitSecondForRank)
                    .Append(_currentRank.transform.DOScale(new Vector3(1f, 1f, 1f), 1.5f))
                    .Join(_rankRect.DOAnchorPos(_pos, 1.5f));
        }
    }

    /// <summary> カットシーン演出(昇格戦解放時に呼ぶ) </summary>
    public void CutSceneLike()
    {
        var sequence = DOTween.Sequence();

                //1, Panel表示
        sequence.Append(_cutPanels[0].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f))
                .Join(_cutPanels[1].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f))
                .AppendCallback(() =>
                {
                    //出現させるのと一緒に音を流す
                    SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Damage");
                })
                .AppendInterval(_waitSecondCutStaging)

                //2, Text表示
                .Append(_cutTexts[0].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f))
                .Join(_cutTexts[1].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f))
                .AppendCallback(() =>
                {
                    SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Damage");
                })
                .AppendInterval(_waitSecondCutStaging)

                //3, BossImage表示
                .Append(_cutPanels[2].transform.DOScale(new Vector3(2.2f, 2.2f, 2.2f), 0.2f))
                 .AppendCallback(() =>
                 {
                     SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Damage");
                 })
                .AppendInterval(_waitSecondCutStaging)

                //4, フェードで消す
                .AppendCallback(() =>
                {
                    foreach (var panel in _cutPanels)
                    {
                        panel.DOFade(0f, 1f).OnComplete(() => panel.gameObject.SetActive(false));
                    }

                    foreach (var text in _cutTexts)
                    {
                        text.DOFade(0f, 1f).OnComplete(() => text.gameObject.SetActive(false));
                    }
                    Fade.Instance.FadeIn();
                });
    }

    public void CutSceneLikeBoss()
    {
        var sequence = DOTween.Sequence();

        //1, Panel表示
        sequence.Append(_cutPanels[0].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f))
                .Join(_cutPanels[1].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f))
                .AppendCallback(() =>
                {
                    //出現させるのと一緒に音を流す
                    SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Damage");
                })
                .AppendInterval(_waitSecondCutStaging)

                //2, BossImage表示
                .Append(_cutPanels[2].transform.DOScale(new Vector3(2.2f, 2.2f, 2.2f), 0.2f))
                 .AppendCallback(() =>
                 {
                     SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Damage");
                 })
                .AppendInterval(_waitSecondCutStaging)

                //3, Text表示
                .Append(_cutTexts[0].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f))
                .Join(_cutTexts[1].transform.DOScale(new Vector3(1f, 1f, 1f), 0.2f))
                .AppendCallback(() =>
                {
                    SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Damage");
                })
                .AppendInterval(_waitSecondCutStaging)

                //4, フェードで消す
                .AppendCallback(() =>
                {
                    foreach (var panel in _cutPanels)
                    {
                        panel.DOFade(0f, 1f).OnComplete(() => panel.gameObject.SetActive(false));
                    }

                    foreach (var text in _cutTexts)
                    {
                        text.DOFade(0f, 1f).OnComplete(() => text.gameObject.SetActive(false));
                    }
                    Fade.Instance.FadeIn();
                });
    }
}

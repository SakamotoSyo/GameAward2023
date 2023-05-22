using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
{
    #region カット演出系
    [SerializeField] private GameObject _battleSelectPanel = default;
    [SerializeField] private GameObject _cutPanelParent = default;
    [SerializeField] private float _waitSecondCutStaging = 1f;
    [Tooltip("昇格戦挑戦可か")]
    [SerializeField] private bool _isChallengablePromotionMatch = false;

    private bool _isPlayCutMove = false;
    private Image[] _cutPanels = new Image[3];
    private Text[] _cutTexts = new Text[2];
    #endregion

    public static HomeScene Instance = default;

    public bool IsChallengablePromotionMatch => _isChallengablePromotionMatch;

    private void Start()
    {
        SettingsCutPanel();

        Instance = this;
    }

    private void Update()
    {
        //以下test
        if (Input.GetKeyDown(KeyCode.A))
        {
            //カットシーン演出
            Fade.Instance.FadeOut();
        }
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

    /// <summary> カットシーン演出(昇格戦解放時に呼ぶ) </summary>
    public void CutSceneLike()
    {
        if (!_isChallengablePromotionMatch || _isPlayCutMove)
        {
            _battleSelectPanel.SetActive(true);
            return;
        }
        Fade.Instance.FadeOut();
    }

    public void CutMove()
    {
        if (_isPlayCutMove)
        {
            return;
        }

        _battleSelectPanel.SetActive(true);

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

        _isPlayCutMove = true;
    }
}

﻿using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] private UnityEvent _onCompleteFadeIn = default;
    [SerializeField] private UnityEvent _onCompleteFadeOut = default;
    [SerializeField] private Image _fadeImage = default;
    [SerializeField] private float _fadeSpeed = 1f;

    private EventSystem _system = default;

    private void Start()
    {
        _system = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        FadeIn();
    }

    /// <summary> FadeIn:明るくなる方 </summary>
    public void FadeIn()
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(0f, _fadeSpeed).
            OnComplete(() => _onCompleteFadeIn?.Invoke());
    }

    /// <summary> FadeOut:暗くなる方 </summary>
    public void FadeOut()
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(1f, _fadeSpeed).
            OnComplete(() => _onCompleteFadeOut?.Invoke());
    }

    public void FadeOut(string sceneName)
    {
        _fadeImage.gameObject.SetActive(true);
        _fadeImage.DOFade(1f, _fadeSpeed).
            OnComplete(() => SceneManager.LoadScene(sceneName));
    }

    /// <summary> ランク選択からバトル選択のシーンに遷移する時に呼ぶ </summary>
    public void LoadToRankScene()
    {
        //選択したランク名を引き継ぐ
        var button = _system.currentSelectedGameObject;
        var rankLevel = button.transform.GetChild(0).GetComponent<Text>();

        RankScene.Rank = rankLevel.text;
    }
}

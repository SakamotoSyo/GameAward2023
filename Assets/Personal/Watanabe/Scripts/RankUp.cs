using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 経験値up→ランクアップしたらUI切り替え </summary>
public class RankUp : MonoBehaviour
{
    [SerializeField] private Sprite[] _ranks = default;

    [Header("数値関連")]
    [Tooltip("拡大値")]
    [SerializeField] private float _scaleValue = 1f;
    [Tooltip("拡大後、縮小するまで待つ秒数")]
    [SerializeField] private float _waitSecond = 1f;

    private RectTransform _rectTransform = default;
    private Image _image = default;
    private int _index = 0;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        _index = Array.IndexOf(_ranks, _image.sprite);
        Debug.Log(_index);
    }

    private void Update()
    {
        //test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeImage();
        }
    }

    public void ChangeImage()
    {
        //最大ランクでなければ
        if (_index != _ranks.Length - 1)
        {
            var sequence = DOTween.Sequence();

            sequence.AppendCallback(() =>
                    {
                        _index++;
                        _image.sprite = _ranks[_index];
                    })
                    .Append(transform.DOScale(new Vector3(1f, 1f, 1f) * _scaleValue, 0.2f))
                    .AppendInterval(_waitSecond)
                    .Append(transform.DOScale(new Vector3(1f, 1f, 1f), 1.5f))
                    .Join(_rectTransform.DOAnchorPos(new Vector3(-500f, -350f, 0f), 1.5f));
        }
    }
}

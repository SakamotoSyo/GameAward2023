using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DamageTextController : MonoBehaviour
{
    [SerializeField, Tooltip("ダメージのテキスト")]
    private Text _damageText;

    [SerializeField, Tooltip("Y座標への移動距離")]
    private float _distanceY = 0.5f;

    [SerializeField, Tooltip("Y座標への移動時間")]
    private float _distanceYTime = 0.2f;

    [SerializeField, Tooltip("フェード時間")]
    private float _fadeTime = 0.5f;

    private void Start()
    {
        transform.DOLocalMoveY(_distanceY, _distanceYTime);
        _damageText.DOFade(0, _fadeTime)
            .SetDelay(_distanceYTime)
            .OnKill(() => Destroy(gameObject));
    }

    /// <summary>
    /// テキストにダメージを表示する
    /// </summary>
    public void TextInit(float damage)
    {
        _damageText.text = damage.ToString();
    }
}

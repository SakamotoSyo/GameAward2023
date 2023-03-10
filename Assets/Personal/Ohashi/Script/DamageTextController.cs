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

    private int _damage;

    private void Start()
    {
        //仮の条件
        if(_damage >= 100)
        {
            _damageText.DOCounter(0, _damage, 0.2f);
            transform.DOScale(new Vector3(0.02f, 0.02f, 0), 0.2f);
            DamageAnimation();
        }
        else
        {
            DamageAnimation();
        }
    }

    /// <summary>
    /// テキストにダメージを表示する
    /// </summary>
    public void TextInit(int damage)
    {
        _damageText.text = damage.ToString();
        _damage = damage;
    }

    /// <summary>
    /// ダメージテキストのアニメーション
    /// </summary>
    private void DamageAnimation()
    {
        transform.DOLocalMoveY(_distanceY, _distanceYTime);
        _damageText.DOFade(0, _fadeTime)
            .SetDelay(_distanceYTime)
            .OnComplete(() => Destroy(gameObject));
    }
}

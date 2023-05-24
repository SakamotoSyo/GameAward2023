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

    [SerializeField]
    private Image _image;

    [SerializeField]
    private Sprite _normal;

    [SerializeField]
    private Sprite _critical;

    private int _damage;

    private bool _isCritical = false;

    private void Start()
    {
        //仮の条件
        if (_isCritical == true)
        {
            _damageText.color = Color.yellow;
            _image.sprite = _critical;
            BigDamageAnimation();
        }
        else
        {
            _damageText.color = Color.white;
            _image.sprite = _normal;
            DamageAnimation();
        }
    }

    /// <summary>
    /// テキストにダメージを表示する
    /// </summary>
    public void TextInit(int damage, bool isCritical)
    {
        _damageText.text = damage.ToString();
        _damage = damage;
    }

    /// <summary>
    /// ダメージテキストのアニメーション
    /// </summary>
    private void DamageAnimation()
    {
        float f = Random.Range(0.3f, 2f);
        transform.DOLocalMoveY(f, _distanceYTime);
        _damageText.DOFade(0, _fadeTime)
            .SetDelay(_distanceYTime)
            .OnComplete(() => Destroy(gameObject));
    }
    private void BigDamageAnimation()
    {
        _damageText.DOCounter(0, _damage, 0.2f);
        transform.DOScale(new Vector3(0.015f, 0.015f, 0), 0.2f);
        DamageAnimation();
    }
}

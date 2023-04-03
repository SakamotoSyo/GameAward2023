using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestMusyokuSkill : MonoBehaviour
{

    [SerializeField, Tooltip("フェード用のパネル")]
    private Image _fadePanel;

    [SerializeField, Tooltip("エネミーのクラス")]
    private EnemyController _enemyController;

    [SerializeField, Tooltip("プレイヤーを黒くする用のスプライト")]
    private SpriteRenderer _blackPlayer;

    [SerializeField, Tooltip("敵を黒くする用のスプライト")]
    private SpriteRenderer _blackEnemy;

    [SerializeField, Tooltip("敵のふちを青くする用のスプライト")]
    private SpriteRenderer _blueEnemy;

    [SerializeField, Tooltip("背景を黒くする用のスプライト")]
    private SpriteRenderer _backGround;

    [SerializeField, Tooltip("爆発エフェクト")]
    private ParticleSystem _explosionEffect;

    [SerializeField, Tooltip("斬撃エフェクト")]
    private ParticleSystem[] _swordEffects;

    [SerializeField, Tooltip("敵に与えるダメージ数")]
    private int _damege = 10000;

    [Header("DOTweenの設定")]

    [SerializeField, Tooltip("プレイヤーの最終地点")]
    private float _lastMoveX = 8f;

    [SerializeField, Tooltip("プレイヤーの移動にかかる時間")]
    private float _lastMoveXTime = 0.3f;

    [SerializeField, Tooltip("プレイヤーの元の位置")]
    private float _backMoveX = -5.5f;

    [SerializeField, Tooltip("プレイヤーが元の位置に戻るまでの時間")]
    private float _backMoveXTime = 0.5f;

    [SerializeField, Tooltip("プレイヤーの傾く角度")]
    private float _playerRotateZ = -11f;

    [SerializeField, Tooltip("プレイヤーを傾ける時間")]
    private float _playerRotateZTime = 0.5f;

    [SerializeField, Tooltip("プレイヤーが元の角度に戻るまでの時間")]
    private float _playerBackRotateZTime = 0.5f;

    [SerializeField, Tooltip("パネルフェードの時間")]
    private float _panelFadeTime = 1f;

    [SerializeField, Tooltip("全体のフェード時間")]
    private float _fadeTime = 1f;

    [SerializeField, Tooltip("フェードアウト後のインターバル")]
    private float _fadeInterval = 0.3f;

    [SerializeField, Tooltip("ダメージ後のインターバル")]
    private float _damegeInterval = 0.5f;

    [SerializeField, Tooltip("移動後のインターバル")]
    private float _moveInterval = 0.5f;

    private int _count = 1;

    /// <summary>
    /// 無職転生のスキル
    /// </summary>
    public void Skill()
    {
        _count = 0;
        _fadePanel.enabled = true;

        var sequence = DOTween.Sequence();
        //フェードアウトしているときにプレイヤーを傾ける
        sequence.Append(_fadePanel.DOFade(1f, _panelFadeTime))
            .Join(transform.DORotate(new Vector3(0, 0, _playerRotateZ), _playerRotateZTime));
        //待つ
        sequence.AppendInterval(_fadeInterval);

        //全体の色を黒くしてフェードをやめる
        sequence.Append(_blackPlayer.DOFade(1, 0))
            .Join(_blackEnemy.DOFade(1, 0))
            .Join(_blueEnemy.DOFade(1, 0))
            .Join(_backGround.DOFade(0.9f, 0))
            .Join(_fadePanel.DOFade(0, 0));
        //プレイヤーを動かす
        sequence.Append(transform.DOMoveX(_lastMoveX, _lastMoveXTime));
        //待つ
        sequence.AppendInterval(_moveInterval);
        //全体の色を元に戻す
        sequence.Append(_blackPlayer.DOFade(0, _fadeTime))
            .Join(_blackEnemy.DOFade(0, _fadeTime))
            .Join(_blueEnemy.DOFade(0, _fadeTime))
            .Join(_backGround.DOFade(0, _fadeTime));
        //敵のダメージのメソッドを呼ぶ
        sequence.AppendCallback(() =>
        {
            _explosionEffect.Play();
            _enemyController.AddDamage(_damege);
        });
        //待つ
        sequence.AppendInterval(_damegeInterval);
        //プレイヤーを元に戻す
        sequence.Append(transform.DORotate(Vector3.zero, _playerBackRotateZTime));
        //もとの位置に戻る
        sequence.Append(transform.DOMoveX(_backMoveX, _backMoveXTime));
        //フェードのパネルを非アクティブにする
        sequence.AppendCallback(() => _fadePanel.enabled = false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //接触したら斬撃Effectを出す
        if (collision.gameObject.TryGetComponent<IAddDamage>(out IAddDamage addDamage) && _count == 0)
        {
            _count++;
            foreach (var swordEffect in _swordEffects)
            {
                swordEffect.Play();
            }
        }
    }
}

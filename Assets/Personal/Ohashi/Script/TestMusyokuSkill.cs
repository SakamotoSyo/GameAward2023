using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestMusyokuSkill : MonoBehaviour
{
    [SerializeField, Tooltip("フェード用のパネル")]
    private Image _fadePanel;

    [SerializeField]
    private EnemyController _enemyController;

    [SerializeField, Tooltip("プレイヤーを黒くする用のスプライト")]
    private SpriteRenderer _blackPlayer;

    [SerializeField, Tooltip("敵を黒くする用のスプライト")]
    private SpriteRenderer _blackEnemy;

    [SerializeField, Tooltip("敵のふちを青くする用のスプライト")]
    private SpriteRenderer _blueEnemy;

    [SerializeField]
    private SpriteRenderer _backGround;

    [SerializeField]
    private ParticleSystem _explosionEffect;

    [SerializeField]
    private ParticleSystem[] _swordEffects;

    [SerializeField, Tooltip("プレイヤーの最終地点")]
    private float _moveX = 8f;

    [SerializeField, Tooltip("攻撃後インターバル")]
    private float _appendInterval = 0.2f;

    private int _count = 1;

    /// <summary>
    /// 無職転生のスキル
    /// </summary>
    public void Skill()
    {
        _count = 0;
        _fadePanel.enabled = true;
        var sequence = DOTween.Sequence();
        //フェードアウト
        sequence.Append(_fadePanel.DOFade(1f, 1f))
            .Join(transform.DORotate(new Vector3(0, 0, -11), 0.5f));
        //待つ
        sequence.AppendInterval(0.3f);
        //全体の色を黒くしてフェードをやめる
        sequence.AppendCallback(() =>
        {
            _backGround.color = new Color(0, 0, 0, 0.9f);
            _blackPlayer.enabled = true;
            _blackEnemy.enabled = true;
            _blueEnemy.enabled = true;
            _fadePanel.color = Color.clear;
        });

        sequence.Append(_blackPlayer.DOFade(1, 0))
            .Join(_blackEnemy.DOFade(1, 0))
            .Join(_blueEnemy.DOFade(1, 0));
        //プレイヤーを動かす
        sequence.Append(transform.DOMoveX(_moveX, 0.3f));
        //待つ
        sequence.AppendInterval(0.5f);

        sequence.Append(_blackPlayer.DOFade(0, 1f))
            .Join(_blackEnemy.DOFade(0, 1f))
            .Join(_blueEnemy.DOFade(0, 1f))
            .Join(_backGround.DOFade(0, 1f));
        //全体の色を元に戻す
        sequence.AppendCallback(() =>
        {
            _blackPlayer.enabled = false;
            _blackEnemy.enabled = false;
            _blueEnemy.enabled = false;
        });
        //待つ
        //sequence.AppendInterval(_appendInterval);
        //敵のダメージのメソッドを呼ぶ
        sequence.AppendCallback(() =>
        {
            _explosionEffect.Play();
            _enemyController.AddDamage(10000);
        });
        //待つ
        sequence.AppendInterval(0.5f);
        sequence.Append(transform.DORotate(Vector3.zero, 0.5f));
        //もとの位置に戻る
        sequence.Append(transform.DOMoveX(-5.5f, 0.5f));
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

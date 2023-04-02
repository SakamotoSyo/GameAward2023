using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestMusyokuSkill : MonoBehaviour
{
    [SerializeField, Tooltip("フェード用のパネル")]
    private Image _fadePanel;

    [SerializeField]
    private EnemyController _enemyController;

    [SerializeField, Tooltip("プレイヤーを黒くする用のオブジェクト")]
    private GameObject _blackPlayer;

    [SerializeField, Tooltip("を黒くする敵を用のオブジェクト")]
    private GameObject _blackEnemy;

    [SerializeField]
    private SpriteRenderer _backGround;

    [SerializeField]
    private ParticleSystem _explosionEffect;

    [SerializeField]
    private ParticleSystem[] _swordEffects;

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
        sequence.Append(_fadePanel.DOFade(1f, 1f));
        //全体の色を黒くしてフェードをやめる
        sequence.AppendCallback(() =>
        {
            _backGround.color = new Color(0.1f, 0.1f, 0.1f, 1);
            _blackPlayer.SetActive(true);
            _blackEnemy.SetActive(true);
            _fadePanel.color = new Color(0, 0, 0, 0);
        });
        //プレイヤーを動かす
        sequence.Append(transform.DOMoveX(5.5f, 0.5f));
        //待つ
        sequence.AppendInterval(1f);
        //全体の色を元に戻す
        sequence.AppendCallback(() =>
        {
            _backGround.color = Color.white;
            _blackPlayer.SetActive(false);
            _blackEnemy.SetActive(false);
        });
        //待つ
        sequence.AppendInterval(0.5f);
        //敵のダメージのメソッドを呼ぶ
        sequence.AppendCallback(() =>
        {
            _explosionEffect.Play();
            _enemyController.AddDamage(10000);
        });
        //待つ
        sequence.AppendInterval(1f);
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

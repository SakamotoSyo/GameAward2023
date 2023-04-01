using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TestMusyokuSkill : MonoBehaviour
{
    [SerializeField, Tooltip("フェード用のパネル")]
    private Image _fadePanel;

    [SerializeField]
    private EnemyController _enemyController;

    /// <summary>
    /// 無職転生のスキル
    /// </summary>
    public void Skill()
    {
        _fadePanel.enabled = true;
        var sequence = DOTween.Sequence();
        //フェードアウト
        sequence.Append(_fadePanel.DOFade(1f, 1f)
            .OnComplete(() => _fadePanel.color = new Color(0, 0, 0, 0)));
        //プレイヤーを動かす
        sequence.Append(transform.DOMoveX(5.5f, 0.5f));
        //待つ
        sequence.AppendInterval(1f);
        //敵のダメージのメソッドを呼ぶ
        sequence.AppendCallback(() => _enemyController.AddDamage(10000));
        //待つ
        sequence.AppendInterval(1f);
        //もとの位置に戻る
        sequence.Append(transform.DOMoveX(-5.5f, 0.5f));
        //フェードのパネルを非アクティブにする
        sequence.AppendCallback(() => _fadePanel.enabled = false);
    }
}

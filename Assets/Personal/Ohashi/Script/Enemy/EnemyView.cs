using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyView : MonoBehaviour
{
    [SerializeField, Tooltip("Helthを表示するテキスト")]
    private Text _healthText;

    [SerializeField]
    private Text _maxHealthText;

    [SerializeField]
    private Image _currentHealthImage;

    /// <summary>
    /// エネミーの残り体力をテキストに表示する
    /// </summary>
    public void HealthText(float health, float max)
    {
        _healthText.text = health.ToString("00");
        _currentHealthImage.DOFillAmount(health / max, 0.5f);
    }

    public void MaxHealthText(float max)
    {
        _maxHealthText.text = max.ToString("00");
    }


}

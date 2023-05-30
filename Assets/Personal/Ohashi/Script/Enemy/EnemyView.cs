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

    private float _max;

    /// <summary>
    /// エネミーの残り体力をテキストに表示する
    /// </summary>
    public void HealthText(float health)
    {
        _maxHealthText.text = health.ToString();
        _currentHealthImage.DOFillAmount(health / _max, 0.5f);
    }

    public void MaxHealthText(float max)
    {
        _healthText.text = max.ToString();
        _max = max;
    }


}

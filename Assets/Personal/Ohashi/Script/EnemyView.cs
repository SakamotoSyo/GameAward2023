using UnityEngine;
using UnityEngine.UI;

public class EnemyView : MonoBehaviour
{
    [SerializeField, Tooltip("Helthを表示するテキスト")]
    private Text _healthText;

    /// <summary>
    /// エネミーの残り体力をテキストに表示する
    /// </summary>
    public void HealthText(float health)
    {
        _healthText.text = health.ToString("00");
    }
}

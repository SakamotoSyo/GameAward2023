using UnityEngine;
using UniRx;

public class EnemyController : MonoBehaviour, IAddDamage
{
    [SerializeField, Tooltip("Å‘å")]
    private float _maxHealth = 5f;

    private ReactiveProperty<float> _health = new();


    private void Start()
    {
        _health.Value = _maxHealth;
        HealthObserver();
    }

    /// <summary>
    /// health‚Ì’l‚ğŠÄ‹‚µA0ˆÈ‰º‚É‚È‚Á‚½‚ÉSubscribe‚·‚é
    /// </summary>
    private void HealthObserver()
    {
        _health
            .Where(health => health <= 0)
            .Subscribe(_ =>
            {
                Debug.Log("Enemy‚Ì‘Ì—Í‚ª0‚É‚È‚Á‚½");
                gameObject.SetActive(false);
            })
            .AddTo(this);
    }

    public void AddDamage(float damage)
    {
        _health.Value -= damage;
    }
}

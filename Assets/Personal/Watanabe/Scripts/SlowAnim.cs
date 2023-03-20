using UnityEngine;

public class SlowAnim : MonoBehaviour
{
    [SerializeField] private float _delayTime = 1f;

    private Animator _anim = default;
    private float _animSpeed = 1f;
    private float _delaying = 0f;
    private bool _isDelay = false;

    public bool IsDelay => _isDelay;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _animSpeed = _anim.speed;
    }

    private void Update()
    {
        if (_isDelay)
        {
            //_delayTime(s)の間Animationの再生をスローにする
            _delaying += Time.unscaledDeltaTime;
            if (_delaying >= _delayTime)
            {
                SetNormal();
                _isDelay = false;
            }
        }
    }

    /// <summary> Animationの再生をスローにする </summary>
    public void DelayAnimation()
    {
        _delaying = 0f;
        _animSpeed = _anim.speed;
        _isDelay = true;
    }

    /// <summary> Animationの再生速度を元に戻す </summary>
    private void SetNormal()
    {
        _anim.speed = _animSpeed;
        _isDelay = false;
    }
}

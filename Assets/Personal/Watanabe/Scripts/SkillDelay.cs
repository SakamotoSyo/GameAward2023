using UnityEngine;

public class SkillDelay : MonoBehaviour
{
    [Tooltip("Time.timeScaleをどれくらい下げるか")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _delayScale = 0.1f;
    [SerializeField] private float _delayTime = 1f;
    [SerializeField] private SlowAnim _slow = default;

    //private Rigidbody2D _rb2d = default;
    private float _delaying = 0f;
    private bool _isDelay = false;

    private void Start()
    {
        //RigidBody.InterpolateをInterpolateにすることで、
        //スロー中の物理演算を滑らかに行う
        //_rb2d = GetComponent<Rigidbody2D>();
        //_rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    private void Update()
    {
        //以下テスト
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_slow.IsDelay)
                _slow.DelayAnimation();
        }

        if (_isDelay)
        {
            _delaying += Time.unscaledDeltaTime;
            if (_delaying >= _delayTime)
            {
                DelayReset();
                _isDelay = false;
            }
        }
    }

    /// <summary> スローモーション </summary>
    public void Delay()
    {
        _delaying = 0f;
        //FixedUpdate()はTime.timeScaleの影響を受ける
        //Update()はTime.timeScaleの影響を受けない
        Time.timeScale = _delayScale;
        _isDelay = true;
        Debug.Log("delay");
    }

    /// <summary> 元に戻す </summary>
    private void DelayReset()
    {
        Time.timeScale = 1f;
        Debug.Log("delay reset");
    }
}

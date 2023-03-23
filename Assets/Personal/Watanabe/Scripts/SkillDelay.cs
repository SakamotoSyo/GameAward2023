using UnityEngine;

public class SkillDelay : MonoBehaviour
{
    [Tooltip("Time.timeScaleをどれくらい下げるか")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _delayScale = 0.1f;
    [Tooltip("Delayの実行時間")]
    [SerializeField] private float _delayTime = 1f;
    [Tooltip("Animationの再生を遅らせる(テスト)")]
    [SerializeField] private SlowAnim _slow = default;

    private float _delaying = 0f;
    private bool _isDelay = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Animationを単体でDelayさせるテスト
            if (_slow != null)
            {
                if (!_slow.IsDelay)
                    _slow.DelayAnimation();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
            Delay();

        if (_isDelay)
        {
            //一定時間経ったらDelay解除
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

    /// <summary> Delayを元に戻す </summary>
    private void DelayReset()
    {
        Time.timeScale = 1f;
        Debug.Log("delay reset");
    }
}

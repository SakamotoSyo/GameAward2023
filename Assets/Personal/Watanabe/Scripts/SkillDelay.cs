using UnityEngine;

public class SkillDelay : MonoBehaviour
{
    [Tooltip("Time.timeScaleをどれくらい下げるか")]
    [Range(0.1f, 1f)]
    [SerializeField] private float _delayScale = 0.1f;

    /// <summary> スローモーション </summary>
    public void Delay()
    {
        //FixedUpdate()はTime.timeScaleの影響を受ける
        //Update()はTime.timeScaleの影響を受けない
        Time.timeScale = _delayScale;
        Debug.LogFormat("delay");
    }

    /// <summary> 元に戻す </summary>
    public void DelayReset()
    {
        Time.timeScale = 1f;
        Debug.LogFormat("delay reset");
    }
}

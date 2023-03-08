using System;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class TimingBar : MonoBehaviour, ISkill
{
    [SerializeField] private RectTransform _timingTransform;
    [Tooltip("タイミングバーの設定する最大値")]
    [SerializeField] private GameObject _timeObj;
    [Tooltip("成功したときの倍率")]
    [SerializeField] private float _successRate = 1.1f;
    [Tooltip("UIのタイミングバーの長さ")]
    [SerializeField] private float _maxTime;
    private float _timingBarWidth;
    [Tooltip("スキルが成功したかどうか")]
    private bool _isSuccess = false;
    [Tooltip("スキルが終わったどうか")]
    private bool _isSkillFinished = false;
    private float _nowTiming;
    private IDisposable _skillDispose;

    private void Start()
    {
        _timingBarWidth = _timingTransform.sizeDelta.x;
        _maxTime = 100;
    }

    public async UniTask StartSkill()
    {
        _timeObj.SetActive(true);
        DOTween.To(() => 0,
        x =>
        {
            _nowTiming = x; 
            _timingTransform.SetWidth(GetWidth(x));
        },
        _maxTime, 1f).SetLoops(-1);

        _skillDispose = this.UpdateAsObservable()
             .Subscribe(_ => TestButtonEvent()).AddTo(this);
        //スキルが終わるまで待機する
        await UniTask.WaitUntil(() => _isSkillFinished == true);
    }

    /// <summary>
    /// スキルを使った結果によって倍率を返す
    /// </summary>
    /// <returns>攻撃力にかける倍率</returns>
    public float SkillResult()
    {
        return _isSuccess ? _successRate : 1;
    }

    public void SkillEnd()
    {
        _isSuccess = false;
        _isSkillFinished = false;
        _timeObj.SetActive(false);
        Debug.Log("購読を終了します");
        _skillDispose.Dispose();
    }

    /// <summary>
    /// スキルが成功したかどうか判定する
    /// </summary>
    public void TestButtonEvent()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (90 <= _nowTiming)
            {
                _isSuccess = true;
                Debug.Log($"成功{_nowTiming}de{GetWidth(90)}");
            }
            else
            {
                Debug.Log($"失敗{_nowTiming}と{_maxTime}");
            }

            _isSkillFinished = true;
        }

    }

    protected float GetWidth(float value)
    {
        float width = Mathf.InverseLerp(0, _maxTime, value);
        return Mathf.Lerp(0, _timingBarWidth, width);
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public static class UIExtensions
{
    /// <summary>
    /// 現在の値をRectにセットする
    /// </summary>
    /// <param name="width"></param>
    public static void SetWidth(this RectTransform rect, float width)
    {
        Vector2 s = rect.sizeDelta;
        s.x = width;
        rect.sizeDelta = s;
    }
}
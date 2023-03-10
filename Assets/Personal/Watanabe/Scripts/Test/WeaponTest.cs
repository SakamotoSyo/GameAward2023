using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;

public class WeaponTest : MonoBehaviour
{
    [SubclassSelector]
    [SerializeReference] private List<ISkill> _skill = new();

    [SerializeField] private RectTransform _timingTransform;
    [SerializeField] private GameObject _timeObj;
    [Tooltip("成功したときの倍率")]
    [SerializeField] private float _successRate = 1.1f;
    [Tooltip("UIのタイミングバーの長さ")]
    private float _timingBarWidth;
    [Tooltip("タイミングバーの設定する最大値")]
    private float _maxTime;
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
        //SkillEnd();
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
                Debug.Log("成功");
            }
            else
            {

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

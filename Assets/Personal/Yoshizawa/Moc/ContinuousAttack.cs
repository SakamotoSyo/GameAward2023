using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 日本語対応
public class ContinuousAttack : MonoBehaviour
{
    private Sequence _sequence = null;
    [SerializeField, Header("敵に近づくのにかかる時間")]
    private float _approachTime = 1f;
    [SerializeField, Header("敵から離れるまでにかかる時間")]
    private float _leaveTime = 1f;
    private float _defaultPositionX = 0f;
    [SerializeField, Header("敵に攻撃を開始するX座標")]
    private float _attackStartPositionX = 1f;
    [SerializeField, Header("攻撃間隔")]
    private float _attackInterval = 0.3f;
    [SerializeField, Header("攻撃回数")]
    private float _numberOfAttacks = 10f;
    private int _count = 0;
    [SerializeField, Header("最後の攻撃を行う時にかかる溜め時間")]
    private float _lastAttackChargeTime = 1f;
    [SerializeField]
    private GameObject[] _slashParticle = null;
    private int _random = 0;
    private float _randomRotationDegree = 0f;

    private void Start()
    {
        _defaultPositionX = transform.position.x;
        //ContinuousAttackSkill(transform);
    }

    public void ContinuousAttackSkill(Transform transform)
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOMoveX(_attackStartPositionX, _approachTime));
        // ↓Add
        SlashAttack();
        _sequence.AppendInterval(_lastAttackChargeTime);
        _sequence.AppendCallback(() =>
        {
            GameObject lastAttackParticle = Instantiate(_slashParticle[0], transform.position, Quaternion.identity);
            lastAttackParticle.transform.localScale = Vector3.one * 1.4f;
        });
        // ↑Add
        _sequence.Append(transform.DOMoveX(_defaultPositionX, _leaveTime)).OnComplete(() =>
        {
            _count = 0;
            _sequence.Kill();
        });
    }

    private GameObject _tempObject = null;

    private void SlashAttack()
    {
        _count++;

        _sequence.AppendInterval(_attackInterval);
        _sequence.AppendCallback(() =>
        {
            _random = Random.Range(0, _slashParticle.Length);
            _randomRotationDegree = Random.Range(10f, 60f);
            _tempObject = _slashParticle[_random];
            Instantiate(_tempObject, transform.position, Quaternion.AngleAxis(_randomRotationDegree, Vector2.right));
        });

        if (_count < _numberOfAttacks)
        {
            SlashAttack();
        }
    }

    //public void ContinuousAttackSkill(Transform transform)
    //{
    //    _sequence.Append(transform.DOMoveX(_attackPositionX, _approachTime));
    //    _sequence.AppendCallback(() =>
    //    {
    //        StartCoroutine(SlashAttack());
    //    });
    //    _sequence.Append(transform.DOMoveX(_defaultPositionX, _leaveTime)).OnComplete(() =>
    //    {
    //        _count = 0;
    //        _isReturnPosition = false;
    //    });
    //}

    //private IEnumerator SlashAttack()
    //{
    //    _count++;
    //    _random = Random.Range(0, _slashParticle.Length);
    //    _randomRotationDegree = Random.Range(10f, 60f);
    //    GameObject go = _slashParticle[_random];
    //    Instantiate(go, transform.position, Quaternion.AngleAxis(_randomRotationDegree, Vector2.right));
    //    bool isComponentAssigned = go.TryGetComponent(out ParticleSystem particle);
    //    yield return new WaitForSeconds(isComponentAssigned ? particle.main.startLifetime.constant : 0.3f);

    //    if (_count < _numberOfAttacks)
    //    {
    //        StartCoroutine(SlashAttack());
    //    }

    //    yield return new WaitForSeconds(1f);
    //    GameObject lastAttackParticle = Instantiate(_slashParticle[0], transform.position, Quaternion.identity);
    //    lastAttackParticle.transform.localScale = Vector3.one * 1.4f;
    //}
}

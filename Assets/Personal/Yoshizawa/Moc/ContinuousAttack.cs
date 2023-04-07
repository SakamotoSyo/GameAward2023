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
    [SerializeField, Header("攻撃開始までの時間")]
    private float _startDelayTime = 1f;
    [SerializeField, Header("攻撃間隔")]
    private float _attackInterval = 0.3f;
    private float _defaultAttackInterval = 0f;
    [SerializeField, Header("攻撃間隔減少値"), Range(0f, 1f)]
    private float _attackIntervalReductionValue = 0.05f;
    [SerializeField, Header("攻撃回数")]
    private float _numberOfAttacks = 10f;
    private int _count = 0;
    [SerializeField, Header("最後の攻撃を行う時にかかる溜め時間")]
    private float _lastAttackChargeTime = 1f;
    [SerializeField, Header("最後に発生させる斬撃の角度")]
    private float _lastAttackDegree = 20f;
    [SerializeField, Header("最後に発生させる斬撃の拡大倍率")]
    private float _lastAttackScale = 2f;
    [SerializeField]
    private GameObject[] _slashParticle = null;
    private int _random = 0;
    private float _randomRotationDegree = 0f;

    private void Start()
    {
        _defaultPositionX = transform.position.x;
        _defaultAttackInterval = _attackInterval;
    }

    public void ContinuousAttackSkill(Transform transform)
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(transform.DOMoveX(_attackStartPositionX, _approachTime));
        _sequence.AppendInterval(_startDelayTime);
        SlashAttack();
        _sequence.AppendInterval(_lastAttackChargeTime);
        _sequence.AppendCallback(() =>
        {
            GameObject lastAttackParticle1 = Instantiate(_slashParticle[0],
                transform.position, Quaternion.AngleAxis(_lastAttackDegree, Vector2.right));
            lastAttackParticle1.transform.localScale = Vector3.one * 2f;
            GameObject lastAttackParticle2 = Instantiate(_slashParticle[2],
                transform.position, Quaternion.AngleAxis(-_lastAttackDegree, Vector2.right));
            lastAttackParticle2.transform.localScale = Vector3.one * 2f;
        });
        _sequence.AppendInterval(_slashParticle[0].GetComponent<ParticleSystem>().main.startLifetime.constant + 0.5f);
        _sequence.Append(transform.DOMoveX(_defaultPositionX, _leaveTime)).OnComplete(() =>
        {
            _count = 0;
            _attackInterval = _defaultAttackInterval;
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
            _attackInterval = Mathf.Clamp(_attackInterval - _attackIntervalReductionValue, 0.1f, _attackInterval);
            SlashAttack();
        }
    }
}

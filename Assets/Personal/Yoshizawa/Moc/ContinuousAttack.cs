using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// 日本語対応
public class ContinuousAttack : MonoBehaviour
{
    [SerializeField]
    private float _approachTime = 1f;
    [SerializeField]
    private float _leaveTime = 1f;
    private float _defaultPositionX = 0f;
    [SerializeField, Tooltip("攻撃を開始する座標")]
    private float _attackPosition = 1f;
    [SerializeField]
    private float _numberOfAttacks = 10f;
    [SerializeField]
    private GameObject[] _slashParticle = null;
    private int random = 0;
    private float _randomRotationDegree = 0f;

    private void Start()
    {
        _defaultPositionX = transform.position.x;
    }

    private void ContinuousAttackSkill(Transform transform)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMoveX(_attackPosition, _approachTime));
        sequence.AppendCallback(() =>
        {
            for (int i = 0; i < _numberOfAttacks; i++)
            {

            }
        });
        sequence.Append(transform.DOMoveX(_defaultPositionX, _leaveTime));
    }

    private IEnumerator SlashAttack()
    {
        random = Random.Range(0, _slashParticle.Length);
        GameObject go = Instantiate(_slashParticle[random]);
        _randomRotationDegree = Random.Range(0f, 65f);
        go.transform.rotation = Quaternion.AngleAxis(transform.rotation.x,
            new Vector3(_randomRotationDegree, transform.rotation.y, transform.position.z));
        yield return new WaitForSeconds(
            go.TryGetComponent(out ParticleSystem particle) ? particle.main.startLifetime.constant : 0.5f);

    }
}

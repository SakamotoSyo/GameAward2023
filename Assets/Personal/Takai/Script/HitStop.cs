using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject[] _enemys;

    [SerializeField] private float _stopTime = 1f;

    Animator _playerAnim;

    private void Start()
    {
        _playerAnim = _player.GetComponent<Animator>();
    }

    public void OnHitStop()
    {
        _playerAnim.speed = 0;

        var seq = DOTween.Sequence();
        seq.SetDelay(_stopTime);
        Damage();

        seq.AppendCallback(() => _playerAnim.speed = 1f);
    }

    private void Damage()
    {
        var seq = DOTween.Sequence();
        foreach(var e in _enemys)
        {
            seq.Append(e.transform.DOShakePosition(_stopTime, 0.15f, 25, fadeOut: false));
        }
        
    }
}

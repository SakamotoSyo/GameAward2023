using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

// 日本語対応
public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource _cameraImpulse = null;
    private float _defaultValue = 0f;

    public bool IsReset { private get; set; } = false;

    private void Start()
    {
        if (TryGetComponent(out CinemachineImpulseSource cinemachineInpulseSource))
        {
            _cameraImpulse = cinemachineInpulseSource;
        }
        else
        {
            _cameraImpulse = gameObject.AddComponent<CinemachineImpulseSource>();
            _cameraImpulse.m_ImpulseDefinition.m_ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform;
            _cameraImpulse.m_ImpulseDefinition.m_ImpulseShape = CinemachineImpulseDefinition.ImpulseShapes.Explosion;
            _cameraImpulse.m_DefaultVelocity.y = 1f;
        }

        _defaultValue = _cameraImpulse.m_DefaultVelocity.y;
    }

    public void CameraShakeMagnitude(float magnification)
    {
        _cameraImpulse.m_DefaultVelocity.y *= magnification;
        _cameraImpulse.GenerateImpulse();

        if (IsReset)
        {
            DOVirtual.DelayedCall(_cameraImpulse.m_ImpulseDefinition.m_ImpulseDuration, () =>
            {
                _cameraImpulse.m_DefaultVelocity.y = _defaultValue;
                IsReset = false;
            });
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// 日本語対応
public class CameraShake : MonoBehaviour
{
    private CinemachineImpulseSource _cameraImpulse = null;

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
            _cameraImpulse.m_ImpulseDefinition.m_ImpulseDuration = 0.2f;
            _cameraImpulse.m_DefaultVelocity.y = 1f;
        }
    }

    public void CameraShakeMagnitude(float magnification)
    {
        _cameraImpulse.m_DefaultVelocity.y *= magnification;
        _cameraImpulse.GenerateImpulse();
    }
}

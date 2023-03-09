using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    [SerializeField] ParticleSystem _particle;
    private void OnEnable()
    {
        _particle.Play();
    }

    private void OnDisable()
    {
        _particle.Stop();
    }
}

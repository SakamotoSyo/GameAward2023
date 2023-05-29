using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SetCinemachineConfiner : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner _confiner;
    // Start is called before the first frame update
    void Start()
    {
        var collider = GameObject.Find("CameraCollider").GetComponent<PolygonCollider2D>();
        _confiner.m_BoundingShape2D = collider;
    }
}

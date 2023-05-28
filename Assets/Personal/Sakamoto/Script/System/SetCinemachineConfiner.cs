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
        _confiner.m_BoundingShape2D = GameObject.Find("CameraCollider").GetComponent<BoxCollider2D>();
    }
}

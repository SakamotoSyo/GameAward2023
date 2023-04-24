using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSample : MonoBehaviour
{
    [SerializeField] MeshManager _meshManager;

    [SerializeField, Tooltip("大剣のサンプル")]
    private List<Vector3> _taikenSample;

    [SerializeField, Tooltip("双剣のサンプル")]
    private List<Vector3> _soukenSample;

    [SerializeField, Tooltip("ハンマーのサンプル")]
    private List<Vector3> _hammerSample;

    [SerializeField, Tooltip("やりのサンプル")]
    private List<Vector3> _yariSample;

    public void SampleTaiken()
    {
        for (int i = 0; i < _taikenSample.Count; i++)
        {
            _meshManager.MyVertices[i] = _taikenSample[i];
            _meshManager.MyMesh.SetVertices(_taikenSample);
        }
    }
}

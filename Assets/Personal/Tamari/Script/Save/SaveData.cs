using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string _prefabName;
    public Vector3[] _myVertices;
    public int[] _myTriangles;
    public int _lowestPosIndex;
    public float _dis;
    public List<Color> _colorList = new List<Color>();
}

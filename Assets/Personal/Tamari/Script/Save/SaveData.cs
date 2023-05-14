using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public string PREHABNAME;
    public Vector3[] MYVERTICES;
    public int[] MYTRIANGLES;
    public int LOWESTPOSINDEX;
    public float DISX;
    public float DISY;
    public List<Color> COLORLIST = new List<Color>();
}

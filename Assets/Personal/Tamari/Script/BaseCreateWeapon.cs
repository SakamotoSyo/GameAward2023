using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCreateWeapon : MonoBehaviour
{
    private MeshRenderer _myRenderer = default;
    private GameObject _go = default;
    protected string _name = default;

    protected SaveData _data = default;
    
    public virtual void Create()
    {
        if (_data._myVertices == null )
        {
            Debug.Log("選んだ武器のセーブデータはありません");
            return;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = _data._myVertices;
        mesh.triangles = _data._myTriangles;
        mesh.SetColors(_data._colorList);

        _go = new GameObject(_data._prefabName);

        MeshFilter meshFilter = _go.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        _myRenderer = _go.AddComponent<MeshRenderer>();
        _myRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
    }
}

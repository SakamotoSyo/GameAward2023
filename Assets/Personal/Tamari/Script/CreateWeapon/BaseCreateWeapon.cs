using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCreateWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject _weaponObj = default;
    [SerializeField]
    private GameObject _weaponHandle = default;
    private MeshRenderer _myRenderer = default;
    private GameObject _childObj = default;

    protected SaveData _data = default;
    
    public virtual void Create()
    {
        if (_data.MYVERTICES == null )
        {
            Debug.Log("選んだ武器のセーブデータはありません");
            return;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = _data.MYVERTICES;
        mesh.triangles = _data.MYTRIANGLES;
        mesh.SetColors(_data.COLORLIST);

        _childObj = new GameObject(_data.PREHABNAME);
        _childObj.transform.parent = _weaponObj.transform;

        MeshFilter meshFilter = _childObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        _myRenderer = _childObj.AddComponent<MeshRenderer>();
        _myRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
    }
}

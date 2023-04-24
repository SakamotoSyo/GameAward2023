#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// TODO : 双剣の時のメッシュ2個作成をする ➔　後回し
/// </summary>
public class MeshManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _centorMark = default;

    [SerializeField, Tooltip("双剣ver")]
    private bool _isSouken = default;

    private MeshFilter _meshFilter = default;

    private Mesh _myMesh;
    public Mesh MyMesh => _myMesh;

    private MeshRenderer _meshRenderer;

    public Material _meshMaterial;

    private Vector3[] _myVertices = default;

    public Vector3[] MyVertices { get { return _myVertices; } }

    private int[] _myTriangles = default;

    private Vector3[] _myNormals = default;

    [SerializeField, Tooltip("最大範囲")]
    private float _maxDelta = default;

    [SerializeField, Tooltip("頂点数")]
    private int _nVertices = 6;

    public int NVertices => _nVertices;

    [SerializeField, Tooltip("中心の座標")]
    private Vector2 _centerPos = default;

    [SerializeField, Tooltip("双剣用の中心の座標")]
    private Vector3 _sCenterPos = default;

    [SerializeField, Tooltip("大きさ"), Range(0, 10)]
    private float _radius = 2f;

    [SerializeField, Tooltip("叩ける範囲")]
    private float _minRange = 1.5f;

    private int _indexNum = default;

    private float _dis = 1000f;

    // public static bool _isFinished;

    private SaveData _saveData;

    public SaveData SaveData => _saveData;

    [SerializeField]
    private List<Color> _setColor = new List<Color>();

    [ContextMenu("Make mesh from model")]

    private void Awake()
    {
        _myMesh = new Mesh();
        _saveData = new SaveData();
        SaveManager.Initialize();
    }

    void Start()
    {
        foreach (var f in SaveManager._weaponFileList)
        {
            SaveManager.Load(f);
        }

        if (!_isSouken)
        {
            CreateMesh();
        }
        else
        {
            CreateSouken("Souken1", _sCenterPos.x, _sCenterPos.y);
            CreateSouken("Souken2", -_sCenterPos.x, _sCenterPos.y);
        }
    }
    void Update()
    {
        _myMesh.SetColors(_setColor);
        _centorMark.transform.position = _centerPos;
        if (Input.GetMouseButtonDown(0))
        {
            Calculation();
            _centerPos = GetCentroid(_myVertices);
        }
    }

    void Calculation()
    {
        //if (_isFinished)
        //{
        //    return;
        //}

        Vector3 mousePos = Input.mousePosition;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;

        for (int i = 0; i < _myVertices.Length; i++)
        {
            float dis = Vector3.Distance(worldPos, _myVertices[i]);
            if (dis < _dis)
            {
                _dis = dis;
                _indexNum = i;
            }
        }

        // タップ位置と近い頂点との距離(ti)
        float tiDis = Vector3.Distance(worldPos, _centerPos);

        // 重心と近い頂点との距離(io)
        float ioDis = Vector3.Distance(_myVertices[_indexNum], _centerPos);

        // 重心とタップ位置との距離(to)
        float toDis = Vector3.Distance(worldPos, _centerPos);

        float disX = worldPos.x - _myVertices[_indexNum].x;
        float disY = worldPos.y - _myVertices[_indexNum].y;

        if (toDis < _minRange && toDis > ioDis)
        {
            Debug.Log("これ以上中に打ち込めません");
            return;
        }

        if (Mathf.Abs(disX) < _radius / 3 && Mathf.Abs(disY) < _radius / 3)
        {
            _myVertices[_indexNum] -= new Vector3(disX, disY, 0);
        }
        else
        {
            Debug.Log($"叩いた場所が一番近い頂点{_indexNum}から離れすぎてます");
        }

        _myMesh.SetVertices(_myVertices);

        _dis = 1000f;
    }
    public void OnSaveData(string weapon)
    {
        if (weapon == "Taiken")
        {
            _saveData._prefabName = weapon;
            _saveData._myVertices = _myVertices;
            _saveData._myTriangles = _myTriangles;
            _saveData._colorList = _setColor;
            SaveManager.Save(SaveManager.TAIKENFILEPATH, _saveData);
        }
        else if (weapon == "Souken")
        {
            _saveData._prefabName = weapon;
            _saveData._myVertices = _myVertices;
            _saveData._myTriangles = _myTriangles;
            _saveData._colorList = _setColor;
            SaveManager.Save(SaveManager.SOUKENFILEPATH, _saveData);
        }
        else if (weapon == "Hammer")
        {
            _saveData._prefabName = weapon;
            _saveData._myVertices = _myVertices;
            _saveData._myTriangles = _myTriangles;
            _saveData._colorList = _setColor;
            SaveManager.Save(SaveManager.HAMMERFILEPATH, _saveData);
        }
        else if (weapon == "Yari")
        {
            _saveData._prefabName = weapon;
            _saveData._myVertices = _myVertices;
            _saveData._myTriangles = _myTriangles;
            _saveData._colorList = _setColor;
            SaveManager.Save(SaveManager.YARIFILEPATH, _saveData);
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("BattleSample");
    }

    public void OnResetSaveData()
    {
        foreach (var f in SaveManager._weaponFileList)
        {
            SaveManager.ResetSaveData(f);
        }
    }

    public void CreateMesh()
    {
        GameObject go = new GameObject("WeaponBase");

        _meshFilter = go.AddComponent<MeshFilter>();

        _meshRenderer = go.AddComponent<MeshRenderer>();

        _myVertices = new Vector3[_nVertices];

        _myNormals = new Vector3[_nVertices];

        // 一辺当たりの中心角の 1 / 2
        float halfStep = Mathf.PI / _nVertices;

        for (int i = 0; i < _nVertices; i++)
        {
            // 中心から i 番目の頂点に向かう角度
            float angle = (i + 1) * halfStep;

            float x = _radius * Mathf.Cos(angle);

            float y = _radius * Mathf.Sin(angle);
            // 下側の頂点の位置と法線
            _myVertices[i].Set(_centerPos.x - x, _centerPos.y - y, 0);
            _myNormals[i] = Vector3.forward;
            i++;
            // 最後の頂点を生成したら終了
            if (i >= _nVertices) break;
            // 上側の頂点の位置と法線
            _myVertices[i].Set(_centerPos.x - x, _centerPos.y + y, 0);
            _myNormals[i] = Vector3.forward;
        }

        _myMesh.SetVertices(_myVertices);

        _myMesh.SetNormals(_myNormals);

        int nPolygons = _nVertices - 2;
        int nTriangles = nPolygons * 3;

        _myTriangles = new int[nTriangles];

        for (int i = 0; i < nPolygons; i++)
        {
            // １つ目の三角形の最初の頂点の頂点番号の格納先
            int firstI = i * 3;
            // １つ目の三角形の頂点番号
            _myTriangles[firstI + 0] = i;
            _myTriangles[firstI + 1] = i + 1;
            _myTriangles[firstI + 2] = i + 2;
            i++;
            // 最後の頂点番号を格納したら終了
            if (i >= nPolygons) break;
            // ２つ目の三角形の頂点番号
            _myTriangles[firstI + 3] = i + 2;
            _myTriangles[firstI + 4] = i + 1;
            _myTriangles[firstI + 5] = i;
        }

        _myMesh.SetTriangles(_myTriangles, 0);

        Vector2[] uvs = new Vector2[_nVertices];

        _myMesh.SetColors(_setColor);
        _meshFilter.sharedMesh = _myMesh;
        _meshRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
        _meshFilter.mesh = _myMesh;
        _meshMaterial.SetInt("GameObject", (int)UnityEngine.Rendering.CullMode.Off);
    }

    // TODO　➔　なにか策を考える
    private void CreateSouken(string name, float sX, float sY)
    {
        GameObject go = new GameObject(name);

        _meshFilter = go.AddComponent<MeshFilter>();

        _meshRenderer = go.AddComponent<MeshRenderer>();

        // _radiuses = new float[_nVertices];   

        _myVertices = new Vector3[_nVertices];

        _myNormals = new Vector3[_nVertices];

        // 一辺当たりの中心角の 1 / 2
        float halfStep = Mathf.PI / _nVertices;

        for (int i = 0; i < _nVertices; i++)
        {
            // 中心から i 番目の頂点に向かう角度
            float angle = (i + 1) * halfStep;

            float x = _radius * Mathf.Cos(angle);

            float y = _radius * Mathf.Sin(angle);
            // 下側の頂点の位置と法線
            _myVertices[i].Set(sX - x, sY - y, 0);
            _myNormals[i] = Vector3.forward;
            i++;
            // 最後の頂点を生成したら終了
            if (i >= _nVertices) break;
            // 上側の頂点の位置と法線
            _myVertices[i].Set(sY - x, sY + y, 0);
            _myNormals[i] = Vector3.forward;
        }

        _myMesh.SetVertices(_myVertices);

        _myMesh.SetNormals(_myNormals);

        int nPolygons = _nVertices - 2;
        int nTriangles = nPolygons * 3;

        _myTriangles = new int[nTriangles];

        for (int i = 0; i < nPolygons; i++)
        {
            // １つ目の三角形の最初の頂点の頂点番号の格納先
            int firstI = i * 3;
            // １つ目の三角形の頂点番号
            _myTriangles[firstI + 0] = i;
            _myTriangles[firstI + 1] = i + 1;
            _myTriangles[firstI + 2] = i + 2;
            i++;
            // 最後の頂点番号を格納したら終了
            if (i >= nPolygons) break;
            // ２つ目の三角形の頂点番号
            _myTriangles[firstI + 3] = i + 2;
            _myTriangles[firstI + 4] = i + 1;
            _myTriangles[firstI + 5] = i;
        }

        _myMesh.SetTriangles(_myTriangles, 0);

        _myMesh.SetColors(_setColor);
        _meshFilter.sharedMesh = _myMesh;
        _meshRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
        _meshFilter.mesh = _myMesh;
        _meshMaterial.SetInt("GameObject", (int)UnityEngine.Rendering.CullMode.Off);
    }

    private Vector3 GetCentroid(Vector3[] vertices)
    {
        Vector3 centroid = Vector3.zero;
        float area = 0f;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 currentVertex = vertices[i];
            Vector3 nextVertex = vertices[(i + 1) % vertices.Length];

            float crossProduct = Vector3.Cross(currentVertex, nextVertex).magnitude;
            float triangleArea = 0.5f * crossProduct;

            area += triangleArea;
            centroid += triangleArea * (currentVertex + nextVertex) / 3f;
        }

        centroid /= area;

        return centroid;
    }
}





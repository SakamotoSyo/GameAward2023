#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;

/// <summary>
/// TODO : 双剣の時のメッシュ2個作成をする
/// </summary>
public class MeshManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _parentObj = default;

    [SerializeField]
    private GameObject _weaponHandle = default;

    private static Vector3 _handlePos = default;

    public static Vector3 HandlePos => _handlePos;

    private Vector3 _lowestPos = default;

    /// <summary>
    /// 重心の位置を表すオブジェクト(後で消す)
    /// </summary>
    [SerializeField]
    private GameObject _jyuusin = default;

    private GameObject _go = default;

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

    private Vector2 _firstCenterPos = default;

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

    private bool _isFinished;

    private SaveData _saveData;
    public SaveData SaveData => _saveData;

    [SerializeField]
    private List<Color> _setColor = new List<Color>();

    public List<Color> SetColor { get { return _setColor; } }

    [SerializeField]
    private string _nextSceneName = default;

    [ContextMenu("Make mesh from model")]

    private void Awake()
    {
        _myMesh = new Mesh();
        _saveData = new SaveData();
        SaveManager.Initialize();
    }

    void Start()
    {
        _isFinished = false;
        _firstCenterPos = _centerPos;
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
        _jyuusin.transform.position = _centerPos;
        _myMesh.SetColors(_setColor);
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

        // 中心と近い頂点との距離(io)
        float ioDis = Vector3.Distance(_myVertices[_indexNum], _centerPos);

        // 中心とタップ位置との距離(to)
        float toDis = Vector3.Distance(worldPos, _centerPos);

        float disX = worldPos.x - _myVertices[_indexNum].x;
        float disY = worldPos.y - _myVertices[_indexNum].y;

        // 叩いた頂点がメッシュの内部に入り込むことを避けたい
        // 現状スタート時の中心からずれなければ入り込まない判定をとれる
        // が、各頂点がずれていくとうまく動かない(中心点が連動しないから)
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

    /// <summary>
    /// 武器のセーブ
    /// </summary>
    /// <param name="weapon"></param>
    public void SaveMesh()
    {
        switch (GameManager.BlacksmithType)
        {
            case WeaponType.GreatSword:
                {
                    BaseSaveMesh(SaveManager.TAIKENFILEPATH);
                }
                break;
            case WeaponType.DualBlades:
                {
                    BaseSaveMesh(SaveManager.SOUKENFILEPATH);
                }
                break;
            case WeaponType.Hammer:
                {
                    BaseSaveMesh(SaveManager.HAMMERFILEPATH);
                }
                break;
            case WeaponType.Spear:
                {
                    BaseSaveMesh(SaveManager.YARIFILEPATH);
                }
                break;
            default:
                {
                    Debug.Log("指定された武器の名前 : " + GameManager.BlacksmithType + " は存在しません");
                }
                return;
        }
    }

    private void BaseSaveMesh(string fileName)
    {
        _saveData._prefabName = GameManager.BlacksmithType.ToString();
        _saveData._myVertices = _myVertices;
        _saveData._myTriangles = _myTriangles;
        _saveData._colorList = _setColor;
        SaveManager.Save(fileName, _saveData);
    }

    /// <summary>
    /// 全武器のセーブデータ削除
    /// </summary>
    public void OnResetSaveData()
    {
        foreach (var f in SaveManager._weaponFileList)
        {
            SaveManager.ResetSaveData(f);
        }
    }

    /// <summary>
    /// メッシュの形を元に戻す
    /// </summary>
    public void ResetMeshShape()
    {
        if (_go == null)
            return;

        Destroy(_go);

        _centerPos = _firstCenterPos;
        CreateMesh();
    }

    public async void ChangeScene()
    {
        if(_isFinished)
        {
            return;
        }
        _isFinished = true;
        SaveMesh();
        SoundManager.Instance.CriAtomPlay(CueSheet.CueSheet_0, "SE_Blacksmith_Finish");
        await UniTask.DelayFrame(2000);
        SceneManager.LoadScene(_nextSceneName);
    }

    public void CreateMesh()
    {
        _go = new GameObject("WeaponBase");
        _go.transform.parent = _parentObj.transform.parent;

        _meshFilter = _go.AddComponent<MeshFilter>();

        _meshRenderer = _go.AddComponent<MeshRenderer>();

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
        _myMesh.SetColors(_setColor);
        _meshFilter.sharedMesh = _myMesh;
        _meshRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
        _meshFilter.mesh = _myMesh;
        _meshMaterial.SetInt("GameObject", (int)UnityEngine.Rendering.CullMode.Off);

        _lowestPos = _myVertices[0];

        for (int i = 0; i < _myVertices.Length; i++)
        {
            if (_lowestPos.y > _myVertices[i].y)
            {
                _lowestPos = _myVertices[i];
            }
        }

        _handlePos = _lowestPos - new Vector3(0, 0.5f, 0);

        _weaponHandle.transform.position = _handlePos;
    }

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

    /// <summary>
    /// メッシュの重心を取得する関数
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
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





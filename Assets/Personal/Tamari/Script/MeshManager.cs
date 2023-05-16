using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

/// <summary>
/// TODO : 鍛冶中に完成予想図を見せることでプレイヤーにわかりやすくする
/// </summary>
public class MeshManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _jyusin = default;

    private int _lowestPosIndex = default;

    private int _rightmostIndex = default;

    private int _leftmostIndex = default;

    private GameObject _go = default;

    public GameObject GO => _go;

    [SerializeField, Tooltip("双剣ver")]
    private bool _isSouken = default;

    private MeshFilter _meshFilter = default;

    private Mesh _myMesh = default;
    public Mesh MyMesh => _myMesh;

    private MeshRenderer _meshRenderer = default;

    public Material _meshMaterial;

    private Vector3[] _myVertices = default;

    public Vector3[] MyVertices { get { return _myVertices; } }

    private int[] _myTriangles = default;

    public int[] MyTriangles => _myTriangles;

    private Vector3[] _myNormals = default;

    [SerializeField, Tooltip("最大範囲")]
    private float _maxDelta = default;

    [SerializeField, Tooltip("頂点数")]
    private int _nVertices = 6;

    public int NVertices => _nVertices;

    private Vector2 _firstCenterPos = default;

    public Vector2 FirstCenterPos => _firstCenterPos;

    [SerializeField, Tooltip("中心の座標")]
    private Vector2 _centerPos = default;
    public Vector2 CentorPos => _centerPos;

    [SerializeField, Tooltip("双剣用の中心の座標")]
    private Vector3 _sCenterPos = default;

    [SerializeField, Tooltip("大きさ"), Range(0, 10)]
    private float _radius = 2f;

    [SerializeField, Tooltip("叩ける範囲")]
    private float _minRange = 1.5f;

    [SerializeField, Tooltip("大きさの限界")]
    private float _sizeLimit = default;

    private float _sizeRight = default;

    private float _sizeLeft = default;

    private int _indexNum = default;

    private float _dis = 1000f;

    public static bool _isFinished;

    private SaveData _saveData = default;
    public SaveData SaveData => _saveData;

    [SerializeField]
    private List<Color> _setColor = new List<Color>();

    [SerializeField]
    private string _nextSceneName = default;

    private float _deltaX = default;
    public float DeltaX => _deltaX;

    private float _deltaY = default;

    public float DeltaY => _deltaY;

    WeaponSaveData _weaponSaveData;

    [SerializeField]
    private GameObject _allPanel = default;

#if UNITY_EDITOR
    public WeaponType _weaponType;
    public bool _isGS;
    public bool _isDB;
    public bool _isH;
    public bool _isS;
#endif

    [ContextMenu("Make mesh from model")]

    private void Awake()
    {
        _myMesh = new Mesh();
        _saveData = new SaveData();
        SaveManager.Initialize();
#if UNITY_EDITOR
        if (_isGS)
        {
            _weaponType = WeaponType.GreatSword;
        }
        if (_isDB)
        {
            _weaponType = WeaponType.DualBlades;
        }
        if (_isH)
        {
            _weaponType = WeaponType.Hammer;
        }
        if (_isS)
        {
            _weaponType = WeaponType.Spear;
        }
#endif
    }

    void Start()
    {
        _isFinished = false;
        _firstCenterPos = _centerPos;

        _rightmostIndex = 4;
        _leftmostIndex = 0;

        _weaponSaveData = new WeaponSaveData();

        if (!_isSouken)
        {
            CreateMesh();
        }
        // 作成後に二つ作成することに成功したから双剣用はもういらないかも
        else
        {
            CreateSouken("Souken1", _sCenterPos.x, _sCenterPos.y);
            CreateSouken("Souken2", -_sCenterPos.x, _sCenterPos.y);
        }
    }
    void Update()
    {
        _jyusin.transform.position = _centerPos;
        if (_isFinished)
        {
            return;
        }

        _myMesh.SetColors(_setColor);

        _centerPos = GetCentroid(_myVertices);

        if (Input.GetMouseButtonDown(0))
        {
            _sizeRight = GetRange().x;
            _sizeLeft = GetRange().y;

            Debug.Log(_sizeRight);
            Debug.Log(_sizeLeft);

            Calculation();
        }
    }

    void Calculation()
    {
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

        _dis = 1000f;

        // 何かの不都合を感じてこれ書いたけどたぶんいらない。また不都合を感じた時のために残しておく
        //if (_indexNum == 2)
        //{
        //    Debug.Log("一番下の頂点は触れません");
        //    return;
        //}

        // タップ位置と近い頂点との距離(ti)
        float tiDis = Vector3.Distance(worldPos, _centerPos);

        // 中心と近い頂点との距離(io)
        float ioDis = Vector3.Distance(_myVertices[_indexNum], _centerPos);

        // 中心とタップ位置との距離(to)
        float toDis = Vector3.Distance(worldPos, _centerPos);

        float disX = worldPos.x - _myVertices[_indexNum].x;
        float disY = worldPos.y - _myVertices[_indexNum].y;

        // メッシュの過剰なめり込みと収縮防止
        if (toDis < _minRange && toDis > ioDis)
        {
            Debug.Log("これ以上中に打ち込めません");
            return;
        }

        // メッシュの過剰な拡大を防止
        if (_sizeRight >= _sizeLimit && ioDis >= toDis && _indexNum == _rightmostIndex
            || _sizeLeft >= _sizeLimit && ioDis >= toDis && _indexNum == _leftmostIndex)
        {
            Debug.Log("これ以上横に大きくできません");
            return;
        }

        if (Mathf.Abs(disX) < _radius / 3 && Mathf.Abs(disY) < _radius / 3)
        {
            _myVertices[_indexNum] -= new Vector3(disX, disY, 0);
            SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Blacksmith");
            _deltaX = _go.transform.position.x - _myVertices[_lowestPosIndex].x;
            _deltaY = _go.transform.position.y - _myVertices[_lowestPosIndex].y;
        }
        else
        {
            Debug.Log($"叩いた場所が一番近い頂点{_indexNum}から離れすぎてます");
        }

        _myMesh.SetVertices(_myVertices);

    }

    /// <summary>
    /// 武器のセーブ
    /// </summary>
    /// <param name="weapon"></param>
    public void SaveMesh()
    {
#if UNITY_EDITOR
        if (_isGS)
        {
            BaseSaveMesh(SaveManager.GREATSWORDFILEPATH, WeaponSaveData.GSData);
        }
        else if (_isDB)
        {
            BaseSaveMesh(SaveManager.DUALBLADESFILEPATH, WeaponSaveData.DBData);
        }
        else if (_isH)
        {
            BaseSaveMesh(SaveManager.HAMMERFILEPATH, WeaponSaveData.HData);
        }
        else if (_isS)
        {
            BaseSaveMesh(SaveManager.SPEARFILEPATH, WeaponSaveData.SData);
        }

        else
#endif
        {
            switch (GameManager.BlacksmithType)
            {
                case WeaponType.GreatSword:
                    {
                        BaseSaveMesh(SaveManager.GREATSWORDFILEPATH, WeaponSaveData.GSData);
                    }
                    break;
                case WeaponType.DualBlades:
                    {
                        BaseSaveMesh(SaveManager.DUALBLADESFILEPATH, WeaponSaveData.DBData);
                    }
                    break;
                case WeaponType.Hammer:
                    {
                        BaseSaveMesh(SaveManager.HAMMERFILEPATH, WeaponSaveData.HData);
                    }
                    break;
                case WeaponType.Spear:
                    {
                        BaseSaveMesh(SaveManager.SPEARFILEPATH, WeaponSaveData.SData);
                    }
                    break;
                default:
                    {
                        Debug.Log("指定された武器の名前 : " + GameManager.BlacksmithType + " は存在しません");
                    }
                    return;
            }
        }

    }

    private void BaseSaveMesh(string fileName, SaveData data)
    {
        data.PREHABNAME = GameManager.BlacksmithType.ToString();
        data.MYVERTICES = _myVertices;
        data.MYTRIANGLES = _myTriangles;
        data.LOWESTPOSINDEX = _lowestPosIndex;
        data.DISX = _go.transform.position.x - _myVertices[_lowestPosIndex].x;
        data.DISY = _go.transform.position.y - _myVertices[_lowestPosIndex].y;
        data.COLORLIST = _setColor;
        SaveManager.Save(fileName, data);
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
        if (_isFinished)
        {
            return;
        }
        if (_go == null)
            return;

        Destroy(_go);

        _centerPos = _firstCenterPos;
        CreateMesh();
    }

    public async void ChangeScene()
    {
        if (_isFinished)
        {
            return;
        }

        var pos = _myVertices[3];
        for (int i = 0; i < _myVertices.Length; i++)
        {
            if (pos.y > _myVertices[i].y)
            {
                pos = _myVertices[i];
                _lowestPosIndex = i;
            }
        }
        _isFinished = true;
        _allPanel.SetActive(true);
        SaveMesh();
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Blacksmith_Finish");
        await UniTask.DelayFrame(500);
        SceneManager.LoadScene(_nextSceneName);
    }

    public void CreateMesh()
    {
        _go = new GameObject("WeaponBase");

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
    }

    private void CreateSouken(string name, float sX, float sY)
    {
        GameObject go = new GameObject(name);

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

    public void ActiveSelectWeapon()
    {
        switch (GameManager.BlacksmithType)
        {
            case WeaponType.GreatSword:
                {

                }
                break;
            case WeaponType.DualBlades:
                {

                }
                break;
            case WeaponType.Hammer:
                {

                }
                break;
            case WeaponType.Spear:
                {

                }
                break;
            default:
                {
                    Debug.Log("指定された武器の名前 : " + GameManager.BlacksmithType + " は存在しません");
                }
                return;
        }

    }

    /// <summary>
    /// メッシュの重心を取得する関数
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns>メッシュの重心座標</returns>
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

    /// <summary>
    /// メッシュの大きさを測る関数
    /// </summary>
    /// <returns>メッシュの横のサイズ</returns>
    private Vector2 GetRange()
    {
        for (int i = 0; i < _myVertices.Length; i++)
        {
            if (_myVertices[_rightmostIndex].x < _myVertices[i].x)
            {
                _rightmostIndex = i;
            }

            if (_myVertices[_leftmostIndex].x > _myVertices[i].x)
            {
                _leftmostIndex = i;
            }
        }

        var disRight = _firstCenterPos.x - _myVertices[_rightmostIndex].x;

        var disLeft = _firstCenterPos.x - _myVertices[_leftmostIndex].x;

        return new Vector2(Mathf.Abs(disRight), Mathf.Abs(disLeft));
    }

}





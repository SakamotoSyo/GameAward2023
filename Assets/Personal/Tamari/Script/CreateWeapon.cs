using UnityEngine;

public class CreateWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject _weaponObj = default;
    [SerializeField]
    private GameObject _weaponHandle = default;
    private MeshRenderer _myRenderer = default;
    private GameObject _childObj = default;

    private SaveData _data = default;

    public void Create()
    {
        if (_data._myVertices == null)
        {
            Debug.Log("選んだ武器のセーブデータはありません");
            return;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = _data._myVertices;
        mesh.triangles = _data._myTriangles;
        mesh.SetColors(_data._colorList);

        _childObj = new GameObject(_data._prefabName);
        _childObj.transform.parent = _weaponObj.transform;

        MeshFilter meshFilter = _childObj.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        _myRenderer = _childObj.AddComponent<MeshRenderer>();
        _myRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
    }
    public void CreateWeapons(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.GreatSword:
                {
                    _data = SaveManager.Load(SaveManager.GREATSWORDFILEPATH);
                }
                break;
            case WeaponType.DualBlades:
                {
                    _data = SaveManager.Load(SaveManager.DUALSWORDFILEPATH);
                }
                break;
            case WeaponType.Hammer:
                {
                    _data = SaveManager.Load(SaveManager.HAMMERFILEPATH);
                }
                break;
            case WeaponType.Spear:
                {
                    _data = SaveManager.Load(SaveManager.SPEARFILEPATH);
                }
                break;
            default:
                {
                    Debug.Log("指定された武器の名前 : " + GameManager.BlacksmithType + " は存在しません");
                }
                return;
        }
        Create();
    }


    /// <summary>
    /// デバッグ用。ボタンでデバッグするときはこれ使おう。
    /// </summary>
    /// <param name="weaponName"></param>
    public void DebugCreate(string weaponName)
    {
        switch (weaponName)
        {
            case "Taiken":
                {
                    _data = SaveManager.Load(SaveManager.GREATSWORDFILEPATH);
                }
                break;
            case "Souken":
                {
                    _data = SaveManager.Load(SaveManager.DUALSWORDFILEPATH);
                }
                break;
            case "Hammer":
                {
                    _data = SaveManager.Load(SaveManager.HAMMERFILEPATH);
                }
                break;
            case "Yari":
                {
                    _data = SaveManager.Load(SaveManager.SPEARFILEPATH);
                }
                break;
            default:
                {
                    Debug.Log("指定された武器の名前 : " + weaponName + " は存在しません");
                }
                return;
        }
        Create();
    }
}

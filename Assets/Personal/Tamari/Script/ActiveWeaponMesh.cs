using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWeaponMesh : MonoBehaviour
{
    public GameObject GSImage => _gsImage;

    public GameObject DBImageR => _dbImageR;

    public GameObject DBImageL => _dbImageL;

    public GameObject HImage => _hImage;

    public GameObject SImage => _sImage;

    private GameObject _gsImage = default;

    private GameObject _dbImageR = default;

    private GameObject _dbImageL = default;

    private GameObject _hImage = default;

    private GameObject _sImage = default;

    private GameObject _gsPos = default;

    [SerializeField, Tooltip("双剣(右)生成ポジション")]
    private GameObject _dbPosR = default;

    [SerializeField, Tooltip("双剣(左)生成ポジション")]
    private GameObject _dbPosL = default;

    [SerializeField, Tooltip("ハンマー生成ポジション")]
    private GameObject _hPos = default;

    [SerializeField, Tooltip("やり生成ポジション")]
    private GameObject _sPos = default;

    [SerializeField, Tooltip("大剣の回転角度")]
    private float _gsRotateAngle = 0;

    [SerializeField, Tooltip("双剣の回転角度")]
    private float _dbRotateAngle = 0;

    [SerializeField, Tooltip("ハンマーの回転角度")]
    private float _hRotateAngle = 0;

    [SerializeField, Tooltip("やりの回転角度")]
    private float _sRotateAngle = 0;

    [SerializeField]
    private List<Color> _setColorList = new List<Color>();

    [SerializeField]
    private float _size = default;

    [SerializeField]
    private ActorGenerator _generator;

    private PlayerStatus _status;

    private WeaponSaveData _weaponSaveData = default;

    private void Awake()
    {
        if (_generator != null)
        {
            _status = _generator.PlayerController.PlayerStatus;
        }
        _weaponSaveData = new WeaponSaveData();
        _gsImage = GameObject.Find("GS");
        _dbImageR = GameObject.Find("DBR");
        _dbImageL = GameObject.Find("DBL");
        _hImage = GameObject.Find("Hammer");
        _sImage = GameObject.Find("spear");
        // 武器表示をしたいときに呼ぶ
        ActiveWeapon();
    }
    private void Start()
    {
        var GsPlayer = GameObject.Find("Hero_Sword");
        var DbPlayer = GameObject.Find("Hero_TwinSword");
        var HPlayer = GameObject.Find("Hero_Hammer");
        var SPlayer = GameObject.Find("Hero_Spear");
        Debug.Log(GsPlayer);
        GsPlayer.SetActive(false);
        DbPlayer.SetActive(false);
        HPlayer.SetActive(false);
        SPlayer.SetActive(false);
    }

    public void ActiveWeapon()
    {
        //for (int i = 0; i < _status.WeaponDatas.Length; i++)
        //{
        //    if (_status.WeaponDatas[i].WeaponType == WeaponType.GreatSword)
        //    {
        //        BaseActiveWeapon(_gsImage, _gsPos, WeaponSaveData.GSData);
        //    }
        //    else if (_status.WeaponDatas[i].WeaponType == WeaponType.DualBlades)
        //    {
        //        BaseActiveWeapon(_dbImageL, _dbPosL, WeaponSaveData.DBData);

        //        BaseActiveWeapon(_dbImageR, _dbPosR, WeaponSaveData.DBData);

        //    }
        //    else if (_status.WeaponDatas[i].WeaponType == WeaponType.Hammer)
        //    {
        //        BaseActiveWeapon(_hImage, _hPos, WeaponSaveData.HData);
        //    }
        //    else
        //    {
        //        BaseActiveWeapon(_sImage, _sPos, WeaponSaveData.SData);
        //    }
        //}
        BaseActiveWeapon(_gsImage, WeaponSaveData.GSData);

        BaseActiveWeapon(_dbImageR, WeaponSaveData.DBData);

        BaseActiveWeapon(_dbImageL, WeaponSaveData.DBData);

        BaseActiveWeapon(_hImage, WeaponSaveData.HData);

        BaseActiveWeapon(_sImage, WeaponSaveData.SData);
    }
    //TODO:所持している武器に対応した表示をする
    //public void ActiveSelectWeapon()
    //{
    //    switch(_meshManager._weaponType)
    //    {
    //        case WeaponType.GreatSword:
    //            {
    //                BaseActiveWeapon(_gsImage, _gsPos, _meshManager.MyVertices, _meshManager.MyTriangles, _meshManager.DeltaX, _meshManager.DeltaY);
    //            }
    //            break;
    //        case WeaponType.DualBlades:
    //            {
    //                BaseActiveWeapon(_dbImageR, _dbPosR, _meshManager.MyVertices, _meshManager.MyTriangles, _meshManager.DeltaX, _meshManager.DeltaY);

    //                BaseActiveWeapon(_dbImageL, _dbPosL, _meshManager.MyVertices, _meshManager.MyTriangles, _meshManager.DeltaX, _meshManager.DeltaY);
    //            }
    //            break;
    //        case WeaponType.Hammer:
    //            {
    //                BaseActiveWeapon(_hImage, _hPos, _meshManager.MyVertices, _meshManager.MyTriangles, _meshManager.DeltaX, _meshManager.DeltaY);
    //            }
    //            break;
    //        case WeaponType.Spear:
    //            {
    //                BaseActiveWeapon(_sImage, _sPos, _meshManager.MyVertices, _meshManager.MyTriangles, _meshManager.DeltaX, _meshManager.DeltaY);
    //            }
    //            break;
    //        default:
    //            {
    //                Debug.Log("指定された武器の名前 : " + GameManager.BlacksmithType + " は存在しません");
    //            }
    //            return;
    //    }
    //}

    private void BaseActiveWeapon(GameObject weapon, SaveData data)
    {
        if (data.MYVERTICES == null)
        {
            Debug.Log("選んだ武器のセーブデータはありません");
            return;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = data.MYVERTICES;
        mesh.triangles = data.MYTRIANGLES;
        mesh.SetColors(_setColorList);

        if (weapon == _hImage)
        {
            var parentWeapon = GameObject.Find("HWeapon");
            var pos = GameObject.Find("HPos");
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _hRotateAngle);
        }

        else if (weapon == _sImage)
        {
            var parentWeapon = GameObject.Find("SWeapon");

            var pos = GameObject.Find("SpearPos");

            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _sRotateAngle);
        }

        else if (weapon == _dbImageL)
        {
            var parentWeapon = GameObject.Find("DBLWeapon");
            var pos = GameObject.Find("SwordPosL");
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z + 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbRotateAngle);
        }

        else if (weapon == _dbImageR)
        {
            var parentWeapon = GameObject.Find("DBRWeapon");
            var pos = GameObject.Find("SwordPosR");
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x - data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z + 1);

            weapon.transform.position = vec;

            weapon.transform.Rotate(0, 180, 0);
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbRotateAngle);
        }

        else if (weapon == _gsImage)
        {
            var parentWeapon = GameObject.Find("GSWeapon");
            var pos = GameObject.Find("SwordPos");
            if (parentWeapon == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _gsRotateAngle);
        }
        weapon.transform.parent.localScale = new Vector3(_size, _size, _size);
        // weapon.transform.localScale = new Vector3(_size, _size, _size);
        MeshFilter meshFilter = weapon.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = weapon.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
    }

    private void BaseActiveWeapon(GameObject weapon, GameObject pos, SaveData data)
    {
        //if (data.MYVERTICES == null)
        //{
        //    Debug.Log("選んだ武器のセーブデータはありません");
        //    return;
        //}

        Mesh mesh = new Mesh();
        mesh.vertices = data.MYVERTICES;
        mesh.triangles = data.MYTRIANGLES;
        mesh.SetColors(_setColorList);

        if (weapon == _hImage)
        {
            var parentWeapon = GameObject.Find("HWeapon");
            if (parentWeapon == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _hRotateAngle);
        }

        else if (weapon == _sImage)
        {
            var parentWeapon = GameObject.Find("SWeapon");
            var parentWeapon2 = GameObject.Find("Spear");
            if (parentWeapon == null && parentWeapon2 == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _sRotateAngle);
            parentWeapon2.transform.rotation = Quaternion.Euler(0, 0, 29.92f);
            Debug.Log(_sRotateAngle);
        }

        else if (weapon == _dbImageL)
        {
            var parentWeapon = GameObject.Find("DBLWeapon");
            if (parentWeapon == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z + 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbRotateAngle);
        }

        else if (weapon == _dbImageR)
        {
            var parentWeapon = GameObject.Find("DBRWeapon");
            if (parentWeapon == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x - data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z + 1);

            weapon.transform.position = vec;

            weapon.transform.Rotate(0, 180, 0);
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbRotateAngle);
        }

        else if (weapon == _gsImage)
        {
            var parentWeapon = GameObject.Find("GSWeapon");
            if (parentWeapon == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _gsRotateAngle);
        }
        weapon.transform.parent.localScale = new Vector3(_size, _size, _size);
        // weapon.transform.localScale = new Vector3(_size, _size, _size);
        MeshFilter meshFilter = weapon.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = weapon.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
    }
}
//switch (GameManager.BlacksmithType)
//{
//    case WeaponType.GreatSword:
//        {
//            data._myVertices[data._lowestPosIndex] = pos.transform.position;
//        }
//        break;
//    case WeaponType.DualBlades:
//        {
//            data._myVertices[data._lowestPosIndex] = pos.transform.position;
//        }
//        break;
//    case WeaponType.Hammer:
//        {
//            data._myVertices[data._lowestPosIndex] = pos.transform.position;
//        }
//        break;
//    case WeaponType.Spear:
//        {
//            data._myVertices[data._lowestPosIndex] = pos.transform.position;
//        }
//        break;
//    default:
//        {
//            Debug.Log("指定された武器の名前 : " + GameManager.BlacksmithType + " は存在しません");
//        }
//        return;
//}
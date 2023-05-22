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


    [SerializeField, Tooltip("大剣のイメージ")]
    private GameObject _gsImage = default;

    [SerializeField, Tooltip("双剣(右)のイメージ")]
    private GameObject _dbImageR = default;

    [SerializeField, Tooltip("双剣(左)のイメージ")]
    private GameObject _dbImageL = default;

    [SerializeField, Tooltip("ハンマーのイメージ")]
    private GameObject _hImage = default;

    [SerializeField, Tooltip("やりのイメージ")]
    private GameObject _sImage = default;

    [SerializeField, Tooltip("大剣生成ポジション")]
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

    private void Start()
    {
        if (_generator != null)
        {
            _status = _generator.PlayerController.PlayerStatus;
        }
        _weaponSaveData = new WeaponSaveData();

        // 武器表示をしたいときに呼ぶ
        ActiveWeapon();
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


        //BaseActiveWeapon(_gsImage, WeaponSaveData.GSData);

        //BaseActiveWeapon(_dbImageR, WeaponSaveData.DBData);

        //BaseActiveWeapon(_dbImageL, WeaponSaveData.DBData);

        //BaseActiveWeapon(_hImage, WeaponSaveData.HData);

        //BaseActiveWeapon(_sImage, WeaponSaveData.SData);

        BaseActiveWeapon(_sImage, _sPos, WeaponSaveData.SData);
        Debug.Log("A");

        BaseActiveWeapon(_gsImage, _gsPos, WeaponSaveData.GSData);
        Debug.Log("B");
        BaseActiveWeapon(_dbImageR, _dbPosR, WeaponSaveData.DBData);
        Debug.Log("C");
        BaseActiveWeapon(_dbImageL, _dbPosL, WeaponSaveData.DBData);
        Debug.Log("D");
        BaseActiveWeapon(_hImage, _hPos, WeaponSaveData.HData);
        Debug.Log("e");

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

        // 大剣の時はこれ
        // TODO:ちゃんと変数にする、武器ごとの確認をする
        var pos = GameObject.Find("SwordPos");
        var pos2 = GameObject.Find("Sword");
        weapon.transform.parent = pos2.transform;

        // weapon.transform.rotation = pos2.transform.rotation;

        if (weapon == _hImage || weapon == _sImage)
        {
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
        }

        else if (weapon == _dbImageR)
        {
            Vector3 vec = new Vector3(pos.transform.position.x - data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;

            weapon.transform.Rotate(0, 180, 0);
        }

        else
        {
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
        }
        weapon.transform.parent.localScale = new Vector3(_size, _size, _size);
        // weapon.transform.localScale = new Vector3(_size, _size, _size);
        MeshFilter meshFilter = weapon.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = weapon.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));

        // 大剣の時はこれ
        // TODO:ちゃんと変数にする、武器ごとの確認をする
        pos2.transform.rotation = Quaternion.Euler(0, 0, -86.588f);
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
            if(parentWeapon == null)
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
            if (parentWeapon == null)
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
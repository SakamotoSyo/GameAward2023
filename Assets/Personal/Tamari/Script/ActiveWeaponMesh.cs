using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWeaponMesh : MonoBehaviour
{
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
        if(_generator != null)
        {
            _status = _generator.PlayerController.PlayerStatus;
        }
        _weaponSaveData = new WeaponSaveData();

        // 武器表示をしたいときに呼ぶ
        // ActiveWeapon();
    }

    //TODO:所持している武器に対応した表示をする
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


        BaseActiveWeapon(_gsImage, _gsPos, WeaponSaveData.GSData);

        BaseActiveWeapon(_dbImageR, _dbPosR, WeaponSaveData.DBData);

        BaseActiveWeapon(_dbImageL, _dbPosL, WeaponSaveData.DBData);

        BaseActiveWeapon(_hImage, _hPos, WeaponSaveData.HData);

        BaseActiveWeapon(_sImage, _sPos, WeaponSaveData.SData);


    }

    // TODO:鍛冶シーンで完成予想図を表示できるようにする
    public void SelectActiveWeapon()
    {

    }

    private void BaseActiveWeapon(GameObject weapon, GameObject pos, SaveData data)
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

        if (weapon == _hImage || weapon == _sImage)
        {
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
        }

        else if (weapon == _dbImageR)
        {
            Vector3 vec = new Vector3(pos.transform.position.x - data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z);

            weapon.transform.position = vec;

            weapon.transform.Rotate(0, 180, 0);
        }

        else
        {
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z);

            weapon.transform.position = vec;

        }
        weapon.transform.parent.localScale = new Vector3(_size, _size, _size);

        MeshFilter meshFilter = weapon.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = weapon.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
    }

    public void BaseActiveWeapon(GameObject weapon, GameObject pos, Vector3[] _myVec, int[] _myTri, float disX, float disY)
    {
        if (_myVec == null)
        {
            Debug.Log("選んだ武器のセーブデータはありません");
            return;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = _myVec;
        mesh.triangles = _myTri;
        mesh.SetColors(_setColorList);

        if (weapon == _hImage || weapon == _sImage)
        {
            Vector3 vec = new Vector3(pos.transform.position.x + disX,
                pos.transform.position.y + disY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
        }

        else if (weapon == _dbImageR)
        {
            Vector3 vec = new Vector3(pos.transform.position.x - disX,
                pos.transform.position.y + disY, pos.transform.position.z);

            weapon.transform.position = vec;

            weapon.transform.Rotate(0, 180, 0);
        }

        else
        {
            Vector3 vec = new Vector3(pos.transform.position.x + disX,
                pos.transform.position.y + disY, pos.transform.position.z);

            weapon.transform.position = vec;

        }
        weapon.transform.parent.localScale = new Vector3(_size, _size, _size);

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
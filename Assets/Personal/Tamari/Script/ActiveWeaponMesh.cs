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
    private ActorGenerator _generator;

    private PlayerStatus _status;

    private WeaponSaveData _weaponSaveData = default;
    private void Start()
    {
        _status = _generator.PlayerController.PlayerStatus;
        _weaponSaveData = new WeaponSaveData();
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
        //    if (_status.WeaponDatas[i].WeaponType == WeaponType.DualBlades)
        //    {
        //        BaseActiveWeapon(_dbImage, _dbPos, WeaponSaveData.DBData);
        //    }
        //    if (_status.WeaponDatas[i].WeaponType == WeaponType.Hammer)
        //    {
        //        BaseActiveWeapon(_hImage, _hPos, WeaponSaveData.HData);
        //    }
        //    if (_status.WeaponDatas[i].WeaponType == WeaponType.Spear)
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

    private void BaseActiveWeapon(GameObject weapon, GameObject pos, SaveData data)
    {
        if (data._myVertices == null)
        {
            Debug.Log("選んだ武器のセーブデータはありません");
            return;
        }
        Mesh mesh = new Mesh();
        mesh.vertices = data._myVertices;
        mesh.triangles = data._myTriangles;
        mesh.SetColors(_setColorList);

        if(weapon == _hImage || weapon == _sImage)
        {
            Vector3 vec = new Vector3(pos.transform.position.x + data._disX,
                pos.transform.position.y + data._disY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
        }

        else if(weapon == _dbImageR)
        {
            Vector3 vec = new Vector3(pos.transform.position.x - data._disX,
                pos.transform.position.y + data._disY, pos.transform.position.z);

            weapon.transform.position = vec;

            weapon.transform.Rotate(0, 180, 0);
        }

        else
        {
            Vector3 vec = new Vector3(pos.transform.position.x + data._disX,
                pos.transform.position.y + data._disY, pos.transform.position.z);

            weapon.transform.position = vec;

        }
        weapon.transform.parent.localScale = new Vector3(0.4f, 0.4f, 0.4f);

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
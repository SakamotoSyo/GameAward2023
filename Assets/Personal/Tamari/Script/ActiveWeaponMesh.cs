using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWeaponMesh : MonoBehaviour
{
    [SerializeField, Tooltip("大剣のイメージ")]
    private GameObject _gsImage = default;

    [SerializeField, Tooltip("双剣のイメージ")]
    private GameObject _dbImage = default;

    [SerializeField, Tooltip("ハンマーのイメージ")]
    private GameObject _hImage = default;

    [SerializeField, Tooltip("やりのイメージ")]
    private GameObject _sImage = default;

    [SerializeField, Tooltip("大剣生成ポジション")]
    private GameObject _gsPos = default;

    [SerializeField, Tooltip("双剣生成ポジション")]
    private GameObject _dbPos = default;

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

        BaseActiveWeapon(_dbImage, _dbPos, WeaponSaveData.DBData);

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
        
        Vector3 vec = new Vector3(pos.transform.position.x, pos.transform.position.y + data._dis, pos.transform.position.z);

        weapon.transform.position = vec;

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
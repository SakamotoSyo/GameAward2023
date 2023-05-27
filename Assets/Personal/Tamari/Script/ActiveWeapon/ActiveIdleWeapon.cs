using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveIdleWeapon : MonoBehaviour
{
    private GameObject _gsImage = default;

    private GameObject _dbImageR = default;

    private GameObject _dbImageL = default;

    private GameObject _hImage = default;

    private GameObject _sImage = default;

    //private GameObject _gsPos = default;

    //[SerializeField, Tooltip("双剣(右)生成ポジション")]
    //private GameObject _dbPosR = default;

    //[SerializeField, Tooltip("双剣(左)生成ポジション")]
    //private GameObject _dbPosL = default;

    //[SerializeField, Tooltip("ハンマー生成ポジション")]
    //private GameObject _hPos = default;

    //[SerializeField, Tooltip("やり生成ポジション")]
    //private GameObject _sPos = default;

    [SerializeField, Tooltip("大剣の回転角度")]
    private float _gsIdleRotateAngle = 0;

    [SerializeField, Tooltip("双剣右の回転角度")]
    private float _dbRIdleRotateAngle = 0;

    [SerializeField, Tooltip("双剣左の回転角度")]
    private float _dbLIdleRotateAngle = 0;

    [SerializeField, Tooltip("ハンマーの回転角度")]
    private float _hIdleRotateAngle = 0;

    [SerializeField, Tooltip("やりの回転角度")]
    private float _sIdleRotateAngle = 0;

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
        // 武器表示をしたいときに呼ぶ
    }
    private void Start()
    {
        _gsImage = GameObject.Find("WeaponGreatSword");
        _dbImageR = GameObject.Find("WeaponDualBladesR");
        _dbImageL = GameObject.Find("WeaponDualBladesL");
        _hImage = GameObject.Find("WeaponHammer");
        _sImage = GameObject.Find("WeaponSpear");
        ActiveWeapon();
    }

    public void ActiveWeapon()
    {
        Debug.Log(_gsImage);
        BaseActiveWeapon(_gsImage, WeaponSaveData.GSData);

        BaseActiveWeapon(_dbImageR, WeaponSaveData.DBData);

        BaseActiveWeapon(_dbImageL, WeaponSaveData.DBData);

        BaseActiveWeapon(_hImage, WeaponSaveData.HData);

        BaseActiveWeapon(_sImage, WeaponSaveData.SData);
    }
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
            var parentWeapon = GameObject.Find("HammerHandleIdle");

            var pos = GameObject.Find("IdleHammerPos");
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _hIdleRotateAngle);
        }

        else if (weapon == _sImage)
        {
            var parentWeapon = GameObject.Find("SpearHandleIdle");

            var pos = GameObject.Find("IdleSpearPos");

            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _sIdleRotateAngle);
        }

        else if (weapon == _dbImageL)
        {
            var parentWeapon = GameObject.Find("DBHandleIdleL");
            var pos = GameObject.Find("IdleDualBladesPosL");
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z + 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbLIdleRotateAngle);
        }

        else if (weapon == _dbImageR)
        {
            var parentWeapon = GameObject.Find("DBHandleIdleR");
            var pos = GameObject.Find("IdleDualBladesPosR");
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x - data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z + 1);

            weapon.transform.position = vec;

            weapon.transform.Rotate(0, 180, 0);
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbRIdleRotateAngle);
        }

        else if (weapon == _gsImage)
        {
            var parentWeapon = GameObject.Find("GreatSwordHandleIdle");
            var pos = GameObject.Find("IdleGreatSwordPos");
            if (parentWeapon == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _gsIdleRotateAngle);
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
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _hIdleRotateAngle);
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
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _sIdleRotateAngle);
            parentWeapon2.transform.rotation = Quaternion.Euler(0, 0, 29.92f);
            Debug.Log(_sIdleRotateAngle);
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
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbLIdleRotateAngle);
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
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbRIdleRotateAngle);
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
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _gsIdleRotateAngle);
        }
        weapon.transform.parent.localScale = new Vector3(_size, _size, _size);
        // weapon.transform.localScale = new Vector3(_size, _size, _size);
        MeshFilter meshFilter = weapon.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = weapon.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Unlit/VertexColorShader"));
    }
}
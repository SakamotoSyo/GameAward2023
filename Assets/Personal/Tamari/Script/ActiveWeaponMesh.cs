using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveWeaponMesh : MonoBehaviour
{
    private GameObject[] _gsImage = default;

    private GameObject[] _dbImageR = default;

    private GameObject[] _dbImageL = default;

    private GameObject[] _hImage = default;

    private GameObject[] _sImage = default;

    private GameObject[] _gsParent = default;

    private GameObject[] _dbrParent = default;

    private GameObject[] _dblParent = default;

    private GameObject[] _hParent = default;

    private GameObject[] _sParent = default;

    private GameObject[] _gsPos = default;

    private GameObject[] _dbrPos = default;

    private GameObject[] _dblPos = default;

    private GameObject[] _hPos = default;

    private GameObject[] _sPos = default;

    [SerializeField]
    private GameObject _insPos = default;

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
    private float _gsRotateAngle = 0;

    [SerializeField, Tooltip("双剣(右)の回転角度")]
    private float _dbRotateAngleR = 0;

    [SerializeField, Tooltip("双剣(左)の回転角度")]
    private float _dbRotateAngleL = 0;

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

    }
    private void Active()
    {
        _gsImage = GameObject.FindGameObjectsWithTag("GSWeapon");
        _dbImageR = GameObject.FindGameObjectsWithTag("DBRWeapon");
        _dbImageL = GameObject.FindGameObjectsWithTag("DBLWeapon");
        _hImage = GameObject.FindGameObjectsWithTag("HammerWeapon");
        _sImage = GameObject.FindGameObjectsWithTag("SpearWeapon");

        _gsParent = GameObject.FindGameObjectsWithTag("GSParent");
        _dbrParent = GameObject.FindGameObjectsWithTag("DBRParent");
        _dblParent = GameObject.FindGameObjectsWithTag("DBLParent");
        _hParent = GameObject.FindGameObjectsWithTag("HammerParent");
        _sParent = GameObject.FindGameObjectsWithTag("SpearParent");

        _gsPos = GameObject.FindGameObjectsWithTag("SwordPos");
        _dbrPos = GameObject.FindGameObjectsWithTag("DBRPos");
        _dblPos = GameObject.FindGameObjectsWithTag("DBLPos");
        _hPos = GameObject.FindGameObjectsWithTag("HammerPos");
        _sPos = GameObject.FindGameObjectsWithTag("SpearPos");

        Debug.Log(_gsImage.Length);
        Debug.Log(_gsParent.Length);
        Debug.Log(_gsPos.Length);
    }
    private void Start()
    {
        var GsPlayer = GameObject.FindGameObjectsWithTag("Sword");
        var DbPlayer = GameObject.FindGameObjectsWithTag("TwinSword");
        var HPlayer = GameObject.FindGameObjectsWithTag("Hammer");
        var SPlayer = GameObject.FindGameObjectsWithTag("Spear");

        Active();

        foreach (var gs in GsPlayer)
        {
            gs.gameObject.SetActive(false);
        }
        foreach (var db in DbPlayer)
        {
            db.gameObject.SetActive(false);
        }
        foreach (var h in HPlayer)
        {
            h.gameObject.SetActive(false);
        }
        foreach (var s in SPlayer)
        {
            s.gameObject.SetActive(false);
        }
        
        // 武器表示をしたいときに呼ぶ
        ActiveWeapon();
    }

    public void ActiveWeapon()
    {
        for(int i = 0; i < _gsImage.Length; i++)
        {
            BaseActiveWeapon(_gsImage[i], WeaponSaveData.GSData, i);
        }

        for (int i = 0; i < _dbImageR.Length; i++)
        {
            BaseActiveWeapon(_dbImageR[i], WeaponSaveData.DBData, i);
        }

        for (int i = 0; i < _dbImageL.Length; i++)
        {
            BaseActiveWeapon(_dbImageL[i], WeaponSaveData.DBData, i);
        }

        for (int i = 0; i < _hImage.Length; i++)
        {
            BaseActiveWeapon(_hImage[i], WeaponSaveData.HData, i);
        }

        for (int i = 0; i < _sImage.Length; i++)
        {
            BaseActiveWeapon(_sImage[i], WeaponSaveData.SData, i);
        }
    }

    private void BaseActiveWeapon(GameObject weapon, SaveData data, int indexNum)
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

        if (weapon == _hImage[indexNum])
        {
            var parentWeapon = _hParent[indexNum];
            var pos = _hPos[indexNum];
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _hRotateAngle);
        }

        else if (weapon == _sImage[indexNum])
        {
            var parentWeapon = _sParent[indexNum];
            var pos = _sPos[indexNum];

            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z - 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _sRotateAngle);
        }

        else if (weapon == _dbImageL[indexNum])
        {
            var parentWeapon = _dblParent[indexNum];
            var pos = _dblPos[indexNum];
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x + data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z + 1);

            weapon.transform.position = vec;
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbRotateAngleL);
        }

        else if (weapon == _dbImageR[indexNum])
        {

            var parentWeapon = _dbrParent[indexNum];
            var pos = _dbrPos[indexNum];
            if (parentWeapon == null || pos == null)
            {
                return;
            }
            Vector3 vec = new Vector3(pos.transform.position.x - data.DISX,
                pos.transform.position.y + data.DISY, pos.transform.position.z + 1);

            weapon.transform.position = vec;

            weapon.transform.Rotate(0, 180, 0);
            parentWeapon.transform.rotation = Quaternion.Euler(0, 0, _dbRotateAngleR);
        }

        else if (weapon == _gsImage[indexNum])
        {
            var parentWeapon = _gsParent[indexNum];
            var pos = _gsPos[indexNum];
            if (parentWeapon == null || pos == null)
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
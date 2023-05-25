using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    WeaponType _weaponType;

    GameObject _weaponBase = default;

    private int _indexNum = 0;

    [SerializeField, Tooltip("完成後のフェードイン用Image")]
    Image _finishImage = default;

    [SerializeField]
    MeshManager _meshManager;

    [SerializeField]
    private string _nextSceneName = default;

    [Header("常に置いてあるUI")]

    [SerializeField]
    private Button _finishButton = default;

    [SerializeField]
    private Button _resetButton = default;

    [SerializeField]
    private Button _sampleCreateButton = default;

    [SerializeField]
    private Text _weaponTypeText = default;

    [Header("リセット時に確認用に出すパネルに必要な情報")]

    [SerializeField]
    private GameObject _allPanel = default;

    [SerializeField]
    private Text _checkTextForReset = default;

    [SerializeField]
    private GameObject _panelForReset = default;

    [SerializeField]
    private Button _okButtonForReset = default;

    [SerializeField]
    private Button _cancelButtonForReset = default;


    [Header("サンプル作成時に確認用に出すパネルに必要な情報")]

    [SerializeField]
    private Text _checkTextForSample = default;

    [SerializeField]
    private GameObject _panelForSample = default;

    [SerializeField]
    private Button _okButtonForSample = default;

    [SerializeField]
    private Button _cancelButtonForSample = default;

    [Header("武器を選ぶ時に表示するパネル")]

    [SerializeField, Tooltip("選択するパネル")]
    private GameObject _firstPanel = default;

    private void Start()
    {
        _allPanel.SetActive(false);
        _panelForReset.SetActive(false);
        _panelForSample.SetActive(false);
        _weaponBase = GameObject.Find("MeshManager");
    }

    public void OnSelect(int num)
    {
        if(num == 0)
        {
            _indexNum = num;
        }
        else if(num == 1)
        {
            _indexNum = num;
        }
        else if (num == 2)
        {
            _indexNum = num;
        }
        else if (num == 3)
        {
            _indexNum = num;
        }
    }
    public void SelectWeapon()
    {
        if (_indexNum == 0)
        {
            _meshManager._weaponType = WeaponType.GreatSword;
            _weaponTypeText.text = "大剣";
        }
        else if (_indexNum == 1)
        {
            _meshManager._weaponType = WeaponType.DualBlades;
            _weaponTypeText.text = "双剣";
        }
        else if (_indexNum == 2)
        {
            _meshManager._weaponType = WeaponType.Hammer;
            _weaponTypeText.text = "ハンマー";
        }
        else if (_indexNum == 3)
        {
            _meshManager._weaponType = WeaponType.Spear;
            _weaponTypeText.text = "槍";
        }
        _firstPanel.SetActive(false);
        _meshManager.CreateMesh();
    }
    //public void SelectWeapon(string weapon)
    //{
    //    if(weapon == WeaponType.GreatSword.ToString())
    //    {
    //        _meshManager._weaponType = WeaponType.GreatSword;
    //        _weaponTypeText.text = "大剣";
    //    }
    //    else if(weapon == WeaponType.DualBlades.ToString())
    //    {
    //        _meshManager._weaponType = WeaponType.DualBlades;
    //        _weaponTypeText.text = "双剣";
    //    }
    //    else if(weapon == WeaponType.Hammer.ToString())
    //    {
    //        _meshManager._weaponType = WeaponType.Hammer;
    //        _weaponTypeText.text = "ハンマー";
    //    }
    //    else if (weapon == WeaponType.Spear.ToString())
    //    {
    //        _meshManager._weaponType = WeaponType.Spear;
    //        _weaponTypeText.text = "やり";
    //    }
    //    _firstPanel.SetActive(false);
    //    _meshManager.CreateMesh();
    //}

    public void SwitchCheckForReset(bool flag)
    {
        Debug.Log("リセット確認のパネル表示");
        _allPanel.SetActive(flag);
        _panelForReset.SetActive(flag);
        _weaponBase.gameObject.SetActive(!flag);
    }

    public void SwitchCheckForSample(bool flag)
    {
        Debug.Log("サンプル生成確認のパネル表示");
        _allPanel.SetActive(flag);
        _panelForSample.SetActive(flag);
        _weaponBase.gameObject.SetActive(!flag);
    }

    public async void ChangeScene()
    {
        if (MeshManager._isFinished)
        {
            return;
        }

        MeshManager._isFinished = true;
        _allPanel.SetActive(true);
        _meshManager.SaveMesh();
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Blacksmith_Finish");
        await _finishImage.DOFade(1.0f, 5f);
        await UniTask.DelayFrame(10);
        SceneManager.LoadScene(_nextSceneName);
    }

    public void ResetMeshShape()
    {
        if (MeshManager._isFinished)
        {
            return;
        }
        if (_meshManager.GO == null)
            return;

        Destroy(_meshManager.GO);

        _meshManager.CentorPos = _meshManager.FirstCenterPos;
        _meshManager.CreateMesh();
    }
}

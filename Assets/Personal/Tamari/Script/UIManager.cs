using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// TODO:持っている武器の中からのみ選択できるようにする
/// </summary>
public class UIManager : MonoBehaviour
{   
    WeaponType _weaponType;

    GameObject _meshManagerObject = default;

    private int? _indexNum = null;

    [SerializeField, Tooltip("完成後のフェードイン用Image")]
    Image _finishImage = default;

    [SerializeField]
    MeshManager _meshManager;

    [SerializeField]
    private string _nextSceneName = default;

    [Header("常に置いてあるUI")]

    [SerializeField]
    private GameObject _defaultWindow = default;

    [SerializeField]
    private Text _weaponTypeText = default;

    [Header("リセット時に確認用に出すパネルに必要な情報")]

    [SerializeField]
    private GameObject _allPanel = default;

    [SerializeField]
    private GameObject _panelForReset = default;

    [Header("サンプル作成時に確認用に出すパネルに必要な情報")]

    [SerializeField]
    private GameObject _panelForSample = default;

    [Header("完成時に確認用に出すパネルに必要な情報")]

    [SerializeField, Tooltip("完成時に確認用に出すパネル")]
    private GameObject _panelForFinish = default;

    [Header("ホームシーンに戻るときの確認用に出すパネルに必要な情報")]

    [SerializeField, Tooltip("ホームシーンに変えるときに確認用に出すパネル")]
    private GameObject _panelForBackHome = default;

    [Header("武器を選ぶ時に表示するパネル")]

    [SerializeField, Tooltip("選択するパネル")]
    private GameObject _firstPanel = default;

    [SerializeField, Tooltip("大剣のフレーム")]
    private GameObject _gsFrame = default;

    [SerializeField, Tooltip("双剣のフレーム")]
    private GameObject _dbFrame = default;

    [SerializeField, Tooltip("ハンマーのフレーム")]
    private GameObject _hFrame = default;

    [SerializeField, Tooltip("槍のフレーム")]
    private GameObject _sFrame = default;

    private void Start()
    {
        _allPanel.SetActive(false);
        _defaultWindow.SetActive(false);
        _panelForReset.SetActive(false);
        _panelForSample.SetActive(false);
        _panelForFinish.SetActive(false);
        _panelForBackHome.SetActive(false);
        ActiveSelectableWeapon();
        _meshManagerObject = GameObject.Find("MeshManager");
    }

    private void ActiveSelectableWeapon()
    {
        _gsFrame.SetActive(false);
        _dbFrame.SetActive(false);
        _hFrame.SetActive(false);
        _sFrame.SetActive(false);

        if (GameManager.PlayerSaveData == null)
        {
            Debug.Log("所持している武器が無いです");
            _gsFrame.SetActive(true);
            return;
        }

        for (int i = 0; i < GameManager.PlayerSaveData.WeaponArray.Length; i++)
        {
            if (GameManager.PlayerSaveData.WeaponArray[i].WeaponType == WeaponType.GreatSword)
            {
                _gsFrame.SetActive(true);
            }
            else if (GameManager.PlayerSaveData.WeaponArray[i].WeaponType == WeaponType.DualBlades)
            {
                _dbFrame.SetActive(true);
            }
            else if (GameManager.PlayerSaveData.WeaponArray[i].WeaponType == WeaponType.Hammer)
            {
                _hFrame.SetActive(true);
            }
            else if (GameManager.PlayerSaveData.WeaponArray[i].WeaponType == WeaponType.Spear)
            {
                _sFrame.SetActive(true);
            }
            else
            {
                Debug.Log("現在取得している武器はありません");
                _gsFrame.SetActive(true);
                break;
            }
        }
    }

    public void OnSelect(int num)
    {
        if (num == 0)
        {
            _indexNum = num;
        }
        else if (num == 1)
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
        if (_indexNum == null)
        {
            Debug.Log("武器を選んでください");
            return;
        }
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
        _defaultWindow.SetActive(true);
        _meshManager.CreateMesh();
    }
    public void SwitchCheckForReset(bool flag)
    {
        _allPanel.SetActive(flag);
        _panelForReset.SetActive(flag);
        _meshManagerObject.gameObject.SetActive(!flag);
    }

    public void SwitchCheckForSample(bool flag)
    {
        _allPanel.SetActive(flag);
        _panelForSample.SetActive(flag);
        _meshManagerObject.gameObject.SetActive(!flag);
    }
    public void SwitchCheckForFinish(bool flag)
    {
        _allPanel.SetActive(flag);
        _panelForFinish.SetActive(flag);
        _meshManagerObject.gameObject.SetActive(!flag);
    }
    public void SwitchCheckForBack(bool flag)
    {
        _allPanel.SetActive(flag);
        _panelForBackHome.SetActive(flag);
        _meshManagerObject.gameObject.SetActive(!flag);
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
        await _finishImage.DOFade(1.0f, 3f);
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

    public async void BackHome()
    {
        if (MeshManager._isFinished)
        {
            return;
        }

        MeshManager._isFinished = true;
        await _finishImage.DOFade(1.0f, 1.0f);
        await UniTask.DelayFrame(10);
        SceneManager.LoadScene("Home");
    }

}

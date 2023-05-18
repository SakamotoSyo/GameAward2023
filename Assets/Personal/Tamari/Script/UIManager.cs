using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    WeaponType _weaponType;

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

    private void Start()
    {
        _allPanel.SetActive(false);
        _panelForReset.SetActive(false);
        _panelForSample.SetActive(false);
        switch (GameManager.BlacksmithType)
        {
            case WeaponType.GreatSword:
                {
                    _weaponTypeText.text = "大剣";
                }
                break;
            case WeaponType.DualBlades:
                {
                    _weaponTypeText.text = "双剣";
                }
                break;
            case WeaponType.Hammer:
                {
                    _weaponTypeText.text = "ハンマー";
                }
                break;
            case WeaponType.Spear:
                {
                    _weaponTypeText.text = "やり";
                }
                break;
            default:
                {
                    Debug.Log("指定された武器の名前 : " + GameManager.BlacksmithType + " は存在しません");
                }
                return;
        }
    }

    public void SwitchCheckForReset(bool flag)
    {
        Debug.Log("リセット確認のパネル表示");
        _allPanel.SetActive(flag);
        _panelForReset.SetActive(flag);
    }

    public void SwitchCheckForSample(bool flag)
    {
        Debug.Log("サンプル生成確認のパネル表示");
        _allPanel.SetActive(flag);
        _panelForSample.SetActive(flag);
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

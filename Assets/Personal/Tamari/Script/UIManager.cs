using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    WeaponType _weaponType;

    [Header("常に置いてあるボタン")]

    [SerializeField]
    private Button _finishButton = default;

    [SerializeField]
    private Button _resetButton = default;

    [SerializeField]
    private Button _sampleCreateButton = default;



    [Header("リセット時に確認用に出すパネルに必要な情報")]

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
        _panelForReset.SetActive(false);
        _panelForSample.SetActive(false);
    }

    public void SwitchCheckForReset(bool flag)
    {
        _panelForReset.SetActive(flag);
    }

    public void SwitchCheckForSample(bool flag)
    {
        _panelForSample.SetActive(flag);
    }
}

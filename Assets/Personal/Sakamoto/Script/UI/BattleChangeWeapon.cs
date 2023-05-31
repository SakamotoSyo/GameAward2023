using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleChangeWeapon : MonoBehaviour
{
    [SerializeField] private ActorGenerator _generator;
    [SerializeField] private GameObject _weaponUiPanel;
    //private BattleChangeWeaponButton[] _weaponUiButton = new BattleChangeWeaponButton[4];
    [SerializeField] private Button[] _weaponButton = new Button[4];
    [SerializeField] private Text[] _weaponTypeText = new Text[4];
    [SerializeField] private GameObject _selectUi;
    [SerializeField] private BattleStateController _battleStateController;
    private GameObject _buttonInsPos;
    private PlayerStatus _playerStatus;

    private void Start()
    {
        _playerStatus = _generator.PlayerController.PlayerStatus;
        Debug.Log(_playerStatus);
    }

    /// <summary>
    /// 武器を入れ替えるためのUIを表示
    /// </summary>
    public void ChangeWeaponUiOpen()
    {
        _weaponUiPanel.SetActive(true);
        _selectUi.SetActive(false);
        for (int i = 0; i < _weaponButton.Length; i++)
        {

            var num = i;
            if (num >= _playerStatus.WeaponDatas.Length) 
            {
                _weaponButton[i].interactable = false;
                continue;
            }

            if (_playerStatus.WeaponDatas[i].CurrentDurable <= 0)
            {
                _weaponButton[i].interactable = false;
            }
            else
            {
                _weaponButton[i].interactable = true;
            }

            if (_playerStatus.EquipWeapon.WeaponNum != i)
            {
                if(_playerStatus.WeaponDatas[num].WeaponType == WeaponType.GreatSword)
                {
                    _weaponTypeText[i].text = "大剣";
                }
                else if (_playerStatus.WeaponDatas[num].WeaponType == WeaponType.DualBlades)
                {
                    _weaponTypeText[i].text = "双剣";
                }
                else if (_playerStatus.WeaponDatas[num].WeaponType == WeaponType.Hammer)
                {
                    _weaponTypeText[i].text = "ハンマー";
                }
                else if (_playerStatus.WeaponDatas[num].WeaponType == WeaponType.Spear)
                {
                    _weaponTypeText[i].text = "槍";
                }

                _weaponButton[i].onClick.RemoveAllListeners();
                _weaponButton[i].onClick.AddListener(() => OnClickChangeWeapon(_playerStatus.WeaponDatas[num], num));
                _weaponButton[i].onClick.AddListener(() => SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Enter"));
            }
            else 
            {
                _weaponButton[i].interactable = false;
            }
        }
    }

    /// <summary>
    /// ボタンを押されたときの入れ替え処理
    /// </summary>
    /// <param name="weaponData">武器のデータ</param>
    public void OnClickChangeWeapon(WeaponData weaponData, int arrayNum)
    {
        _playerStatus.EquipWeponChange(weaponData, arrayNum);
        _weaponUiPanel.SetActive(false);
        _battleStateController.ActorStateEnd();
    }

    public void CancelButton()
    {
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Cancel");
        _weaponUiPanel.SetActive(false);
        _battleStateController.ActorStateEnd();
    }
}

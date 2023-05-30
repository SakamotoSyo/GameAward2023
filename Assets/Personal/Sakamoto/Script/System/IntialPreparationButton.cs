using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IntialPreparationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private WeaponType _weaponType;
    [SerializeField] private IntialPreparationScript _preparationScript;
    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private Text _infoText;
    [SerializeField] private WeaponData _weaponData;
    private Button _button;
    private bool _isSet = false;

    private void Start()
    {
        _weaponData = new WeaponData(_weaponData.OffensivePower, _weaponData.WeaponWeight, _weaponData.CriticalRate, _weaponData.MaxDurable, 
            WeaponData.AttributeType.None, _weaponData.WeaponType);
        _button = GetComponent<Button>();
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }


    public void IntialSetWeaponData() 
    {
        if (_preparationScript.SetWeaponTypeConfirmation(_weaponData) && !_isSet)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(true);

            //_button.image.color = Color.green;
            SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Enter");

            _isSet = true;
            Debug.Log("SetÇµÇ‹ÇµÇΩ");
        }
        else if (_isSet)
        {
            gameObject.transform.GetChild(1).gameObject.SetActive(false);

            _preparationScript.WeaponDatas.Remove(_weaponData);
            //_button.image.color = Color.white;
            SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Cancel");

            _isSet = false;
            Debug.Log("çÌèúÇµÇ‹ÇµÇΩ");
        }
        else
        {
            Debug.Log("SetèoóàÇ‹ÇπÇÒÇ≈ÇµÇΩ");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Select_Home");

        _infoText.text = DataBaseScript.WeaponDescriptionData[_weaponType];
        _infoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _infoPanel.SetActive(false);
        _infoText.text = ""; 
    }
}

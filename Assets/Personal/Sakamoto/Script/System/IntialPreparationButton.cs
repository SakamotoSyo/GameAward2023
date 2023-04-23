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
    private WeaponData _weaponData;
    private Button _button;
    private bool _isSet = false;

    private void Start()
    {
        _weaponData = new WeaponData(1000, 1000, 50, 1000, WeaponData.AttributeType.None, _weaponType);
        _button = GetComponent<Button>();
    }


    public void IntialSetWeaponData() 
    {
        if (_preparationScript.SetWeaponTypeConfirmation(_weaponData) && !_isSet)
        {
            _button.image.color = Color.green;
            _isSet = true;
            Debug.Log("SetÇµÇ‹ÇµÇΩ");
        }
        else if (_isSet) 
        {
            _preparationScript.WeaponDatas.Remove(_weaponData);
            _button.image.color = Color.white;
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
      //  _infoText.text = DataBaseScript.WeaponDescriptionData[_weaponType];
        _infoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _infoPanel.SetActive(false);
        //_infoText.text = ""; 
    }
}

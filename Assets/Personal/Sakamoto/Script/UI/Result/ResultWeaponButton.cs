using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultWeaponButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text _statusInfoText;
    [SerializeField] private GameObject _outlineObj;
    private ActorGenerator _actorGenerator;
    private WeaponData _weaponData;
    private OreData _oreData;
    private bool _isSetUp = false;


    public void SetUpInfo(WeaponData weaponData, OreData ore)
    {
        _weaponData = weaponData;
        _oreData = ore;
        _isSetUp = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSetUp)
        {
            for (int i = 0; i < _oreData.EnhancedData.Length; i++)
            {
                if (_oreData.EnhancedData[i].EnhanceNum != 0)
                {
                    _statusInfoText.text += _oreData.EnhancedData[i].EnhanceDescription.Replace("+n", " ");
                    _statusInfoText.text += _weaponData.StatusArray[i];
                    _statusInfoText.text += " => ";
                    int num = _oreData.EnhancedData[i].EnhanceNum + (int)_weaponData.StatusArray[i];
                    _statusInfoText.text += "<color=red>" + num + "</color>\n";
                }
                //_statusInfoText.text += _actorGenerator.PlayerController.PlayerStatus.PlayerSkillList;
            }

            if (_weaponData.WeaponType == _oreData.Skill.Weapon) 
            {
                _statusInfoText.text += "<color=blue>" + _oreData.Skill.SkillName + "</color>\n";
            }
        }
     
    }

    public void ActiveOutLine(bool active) 
    {
        _outlineObj.SetActive(active);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _statusInfoText.text = "";
    }

}

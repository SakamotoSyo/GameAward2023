using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResultWeaponButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text _statusInfoText;
    private ActorGenerator _actorGenerator;
    private WeaponData _weaponData;
    private OreData _oreData;


    public void SetUpInfo(WeaponData weaponData, OreData ore)
    {
        _weaponData = weaponData;
        _oreData = ore;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        for (int i = 0; i < _oreData.EnhancedData.Length; i++)
        {
            if (_oreData.EnhancedData[i].EnhanceNum != 0)
            {
                _statusInfoText.text += _oreData.EnhancedData[i].EnhanceDescription.Replace("+n", " ");
                _statusInfoText.text += _weaponData.StatusArray[i];
                _statusInfoText.text += " => ";
                int num = _oreData.EnhancedData[i].EnhanceNum + (int)_weaponData.StatusArray[i];
                _statusInfoText.text += "<color=red>"+ num + "</color>\n";   
            }
            //_statusInfoText.text += _actorGenerator.PlayerController.PlayerStatus.PlayerSkillList;
        }

        _statusInfoText.text += "<color=blue>" + _oreData.Skill.SkillName + "</color>\n";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _statusInfoText.text = "";
    }

}

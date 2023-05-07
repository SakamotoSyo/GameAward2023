using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OreUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private Image _oreImage;
    [SerializeField] private ActorGenerator _actorGenerator;
    [SerializeField] private Text _rearityText;
    [SerializeField] private Text _enhanceText;
    private OreData _data;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _infoPanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _infoPanel.SetActive(false);
    }

    public void SetData(OreData oreData)
    {
        _rearityText.text = oreData.Rarity.ToString();
        for (int i = 0; i < oreData.EnhancedData.Length; i++)
        {
            if (0 == oreData.EnhancedData[i].EnhanceNum) continue;

            _enhanceText.text += oreData.EnhancedData[i].EnhanceDescription;
            if (0 < oreData.EnhancedData[i].EnhanceNum)
            {
                _enhanceText.text += "+";
                _enhanceText.text += oreData.EnhancedData[i].EnhanceNum.ToString();
            }
            else
            {
                _enhanceText.text += oreData.EnhancedData[i].EnhanceNum.ToString();
            }
            _enhanceText.text += " ";

        }
        _enhanceText.text += "\n" + oreData.Skill.SkillName;
        _enhanceText.text += "\n" + oreData.Skill.FlavorText;
        _oreImage.sprite = oreData.OreImage;
    }

    public void ResetPanel()
    {
        _infoPanel.SetActive(false);
        _enhanceText.text += " ";
    }
}

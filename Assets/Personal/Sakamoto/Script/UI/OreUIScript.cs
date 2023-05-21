using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OreUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private GameObject _infoPanel;
    [SerializeField] private Image _oreImage;
    [SerializeField] private Text _rearityText;
    [SerializeField] private Text _enhanceText;
    [SerializeField] private bool _isSelect = false;
    private OreData _data;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isSelect)
        {
            SetData(_data);
        }
        else 
        {
            _infoPanel.SetActive(true);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isSelect)
        {
            _enhanceText.text = "";
        }
        else 
        {
            _infoPanel.SetActive(false);
        }
    }

    public void SetData(OreData oreData)
    {
        _data = oreData;
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

    public void SetOreData(OreData oreData) 
    {
        _data = oreData;
        _oreImage.sprite = oreData.OreImage;
        _rearityText.text = oreData.Rarity.ToString();
    }

    public void ResetPanel()
    {
        _infoPanel.SetActive(false);
        _enhanceText.text += " ";
    }
}

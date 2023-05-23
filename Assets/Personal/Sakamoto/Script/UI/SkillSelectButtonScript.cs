using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSelectButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Text _skillText;
    private SkillBase _skill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentSkill(SkillBase skill) 
    {
        if (!skill) return;
        _skill = skill;
        _skillText.text = skill.SkillName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
      
    }
}


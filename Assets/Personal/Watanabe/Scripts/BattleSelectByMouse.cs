using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleSelectByMouse : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private BattleChangeWeapon _changeWeapon;
    [SerializeField] private BattleSelectUI _selectUI = default;

    [SerializeField] private GameObject _skillInfoPanel = default;
    private int _index = 0;

    private void Start()
    {
        _index = Array.IndexOf(_selectUI.ActionUI, transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((_index != 0 && _selectUI.IsAttackable[_index - 1]) || _index == 0)
        {
            SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Select_Ingame");
            _selectUI.BattleSelect(_index);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if ((_index != 0 && _selectUI.IsAttackable[_index - 1]) || _index == 0)
        {
            SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Enter");
            Debug.Log("attack");
            _selectUI.AttackEvent();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _skillInfoPanel.SetActive(false);
    }
}

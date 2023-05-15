using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleSelectByMouse : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler
{
    private BattleSelectUI _selectUI = default;
    private int _index = 0;

    private void Start()
    {
        _selectUI = transform.parent.GetComponent<BattleSelectUI>();

        _index = Array.IndexOf(_selectUI.ActionUI, transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _selectUI.BattleSelect(_index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("attack");
        _selectUI.Attack();
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleSelectByMouse : MonoBehaviour, IPointerEnterHandler,IPointerClickHandler
{
    [SerializeField] private BattleChangeWeapon _changeWeapon;
    [SerializeField] private BattleSelectUI _selectUI = default;
    private int _index = 0;

    private void Start()
    {

        _index = Array.IndexOf(_selectUI.ActionUI, transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Select");
        _selectUI.BattleSelect(_index);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Enter");
        Debug.Log("attack");
        _changeWeapon.ChangeWeaponUiOpen();
    }
}

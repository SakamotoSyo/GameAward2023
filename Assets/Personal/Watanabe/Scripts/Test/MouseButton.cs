using UnityEngine;
using UnityEngine.EventSystems;

public class MouseButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private ButtonSelect _select = default;
    [SerializeField] private int _index = 0;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _select.UIMoveByMouse(_index);
    }
}

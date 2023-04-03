using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickEvent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent _onClick = default;

    public void OnPointerClick(PointerEventData eventData)
    {
        //オブジェクトクリック時に指定したイベントを実行する
        _onClick?.Invoke();
    }

    public void Test()
    {
        Debug.Log("aaa");
    }
}

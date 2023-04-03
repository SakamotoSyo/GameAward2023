using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickEvent : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent _onClick = default;
    [Tooltip("ステージの情報")]
    [Multiline(6)]
    [SerializeField] private string _info = "";

    public string Info { get => _info; protected set => _info = value;}

    public void OnPointerClick(PointerEventData eventData)
    {
        //オブジェクトクリック時に指定したイベントを実行する
        _onClick?.Invoke();
    }
}

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OreSelect : MonoBehaviour, IPointerClickHandler
{
    [Multiline(5)]
    [SerializeField] private string _parameter = "";
    [Multiline(3)]
    [SerializeField] private string _flavorText = "";
    [SerializeField] private UnityEvent _onClick = default;

    public string Parameter { get => _parameter; protected set => _parameter = value; }
    public string FlavorText { get => _flavorText; protected set => _flavorText = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick?.Invoke();
    }
}

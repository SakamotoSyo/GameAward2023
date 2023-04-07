using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private int _index = 0;
    [Tooltip("このスロットに装備があるか(trueなら装備している)")]
    [SerializeField] private bool _isEquip = false;
    [SerializeField] private UnityEvent _onEquip = default;
    [Tooltip("武器パラメータ(仮)")]
    [Multiline(6)]
    [SerializeField] private string _parameter = "";

    private Color _color = Color.cyan;

    public string Parameter { get => _parameter; protected set => _parameter = value; }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Panelにパラメータ反映
        Click();
    }

    public void Click()
    {
        _onEquip?.Invoke();
        //if (_isEquip)
        //{
        //    _onEquip?.Invoke();
        //}
        SetButtons();
    }

    private void SetButtons()
    {
        //パラメータを取得、反映させる
        gameObject.GetComponent<Image>().color = _color;
    }
}

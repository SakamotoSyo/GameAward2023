using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEvent _onClick = default;
    [SerializeField] private UnityEvent _onEquip = default;
    [SerializeField] private UnityEvent _onChange = default;
    [Tooltip("このスロットに装備があるか")]
    [SerializeField] private bool _isEquip = false;

    private Color _selectable = Color.white;
    private Color _unSelectable = Color.gray;
    private Button[] _equipButtons = new Button[2];

    private void Start()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < _equipButtons.Length; i++)
        {
            _equipButtons[i] = 
                gameObject.transform.GetChild(0).
                gameObject.transform.GetChild(i).GetComponent<Button>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _onClick?.Invoke();
        SetButtons();

        if (_isEquip)
        {
            //していなかったら、武器を装備する
            _onEquip?.Invoke();

            _equipButtons[1].interactable = false;
            _equipButtons[1].GetComponent<Image>().color = _unSelectable;

            _equipButtons[0].interactable = true;
            _equipButtons[0].GetComponent<Image>().color = _selectable;
        }
        else
        {
            //装備していたら、武器の変更が可能
            _onChange?.Invoke();

            _equipButtons[0].interactable = false;
            _equipButtons[0].GetComponent<Image>().color = _unSelectable;

            _equipButtons[1].interactable = true;
            _equipButtons[1].GetComponent<Image>().color = _selectable;
        }
    }

    private void SetButtons()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        for (int i = 0; i < _equipButtons.Length; i++)
        {
            _equipButtons[i].gameObject.SetActive(true);
        }
    }
}

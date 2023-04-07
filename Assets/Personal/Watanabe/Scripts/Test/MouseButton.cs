using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private int _index = 0;

    /// <summary> HomeSceneの場合はこっち </summary>
    private ButtonSelect _select = default;
    /// <summary> 戦闘準備シーンの場合はこっち </summary>
    private SlotUI _slot = default;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            _select = GameObject.Find("ButtonSelect").GetComponent<ButtonSelect>();
        }
        else if (SceneManager.GetActiveScene().name == "BattlePreparation")
        {
            _slot = GameObject.Find("WeaponList").GetComponent<SlotUI>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.TryGetComponent(out Button button))
        {
            _select.UIMoveByMouse(_index);
        }
        else if (gameObject.TryGetComponent(out Image image))
        {
            _slot.UIMoveByMouse(_index);
        }
    }
}

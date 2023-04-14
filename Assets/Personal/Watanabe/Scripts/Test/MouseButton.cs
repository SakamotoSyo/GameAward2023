using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MouseButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private int _index = 0;

    /// <summary> HomeSceneの場合はこっち </summary>
    private ButtonSelect _select = default;
    /// <summary> RankSelectSceneの場合はこっち </summary>
    private RankSelect _rank = default;

    public int Index { get => _index; protected set => _index = value; }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "HomeScene")
        {
            _select = gameObject.transform.GetComponentInParent<ButtonSelect>();
        }
        else if (SceneManager.GetActiveScene().name == "RankSelect")
        {
            _rank = gameObject.transform.GetComponentInParent<RankSelect>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _select?.UIMoveByMouse(_index);
        _rank?.UIMoveByMouse(_index);
    }
}

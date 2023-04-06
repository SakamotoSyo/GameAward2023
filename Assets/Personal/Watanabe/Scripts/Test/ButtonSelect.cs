using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary> HomeSceneのButtonで使う </summary>
public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Button[] _select = new Button[3];
    [SerializeField] private Transform[] _start = new Transform[3];
    [SerializeField] private Transform[] _movePos = new Transform[3];
    [Header("↓確認用")]
    [SerializeField] private int _index = 0;
    [SerializeField] private int _beforeIndex = 0;

    private Selectable _selectable = default;

    public int Index { get => _index; set => _index = value; }
    public Selectable Selected { get => _selectable; set => _selectable = value; }

    private void Start()
    {
        _selectable = _select[0];
        _selectable.Select();

        _selectable.transform.DOLocalMove(_movePos[_index].transform.localPosition, 0.1f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            UIMove(0);
        }
        else if (Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.DownArrow))
        {
            UIMove(1);
        }
    }

    private void UIMove(int num)
    {
        _selectable.transform.DOLocalMove(_start[_index].transform.localPosition, 0.1f);

        if (num == 0)
        {
            _index--;
            if (_index < 0)
            {
                _index = _select.Length - 1;
            }
        }
        else if (num == 1)
        {
            _index++;
            if (_index >= _select.Length)
            {
                _index = 0;
            }
        }
        _selectable = _select[_index];
        _selectable.Select();

        _selectable.transform.DOLocalMove(_movePos[_index].transform.localPosition, 0.1f);
    }

    public void UIMoveByMouse(int index)
    {
        _beforeIndex = _index;

        //条件分岐の判定順の都合上、以下のように書いている
        if (index == 2 && _beforeIndex == 0)
        {
            UIMove(0);
        }
        else if (index == 0 && _beforeIndex == 2)
        {
            UIMove(1);
        }

        else if (index < _beforeIndex)
        {
            UIMove(0);
        }
        else if (index > _beforeIndex)
        {
            UIMove(1);
        }
    }
}

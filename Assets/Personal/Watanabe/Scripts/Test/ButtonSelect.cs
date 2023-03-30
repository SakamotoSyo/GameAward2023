using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Button[] _select = new Button[3];
    [SerializeField] private Transform[] _start = new Transform[3];
    [SerializeField] private Transform[] _movePos = new Transform[3];
    [SerializeField] private int _index = 0;

    
    private Selectable _selectable = default;

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

    public void Test()
    {
        Debug.Log(gameObject.name);
    }
}

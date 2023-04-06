using UnityEngine;
using UnityEngine.UI;

/// <summary> 戦闘準備の武器スロットで使う </summary>
public class SlotUI : MonoBehaviour
{
    [SerializeField] private Image[] _weapons = new Image[4];
    [Header("↓確認用")]
    [SerializeField] private int _index = 0;
    [SerializeField] private int _beforeIndex = 0;

    private Image _select = default;
    public int Index { get => _index; set => _index = value; }

    private void Start()
    {
        _select = _weapons[0];
        ViewUI();
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //HomeSceneに戻る
            Fade.Instance.FadeOut();
        }
    }

    private void UIMove(int num)
    {
        if (num == 0)
        {
            _index--;
            if (_index < 0)
            {
                _index = _weapons.Length - 1;
            }
        }
        else if (num == 1)
        {
            _index++;
            if (_index >= _weapons.Length)
            {
                _index = 0;
            }
        }
        _select = _weapons[_index];
        ViewUI();
    }

    private void ViewUI()
    {
        if (_select.TryGetComponent(out WeaponSlot slot))
        {
            slot.Click();
        }
    }

    public void UIMoveByMouse(int index)
    {
        _beforeIndex = _index;

        //条件分岐の判定順の都合上、以下のように書いている
        if (index == 3 && _beforeIndex == 0)
        {
            UIMove(0);
            Debug.Log("うえ");
        }
        else if (index == 0 && _beforeIndex == 3)
        {
            UIMove(1);
            Debug.Log("した");
        }

        else if (index < _beforeIndex)
        {
            UIMove(0);
            Debug.Log("うえ");
        }
        else if (index > _beforeIndex)
        {
            UIMove(1);
            Debug.Log("した");
        }
    }
}

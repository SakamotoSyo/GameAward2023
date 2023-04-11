using UnityEngine;
using UnityEngine.UI;

public class RankSelect : MonoBehaviour
{
    [Tooltip("各ランクの状態")]
    [SerializeField] private ClearState[] _state = new ClearState[5];
    [SerializeField] private Sprite[] _images = new Sprite[5];

    [SerializeField] private Image _stageImage = default;
    [Header("↓確認用")]
    [SerializeField] private int _index = 0;
    [SerializeField] private int _beforeIndex = 0;

    /// <summary> RankButtonを全て格納しておく </summary>
    private Button[] _buttons = new Button[5];
    private int[] _indexes = new int[5];

    private int _startIndex = 0;
    private Selectable _selectable = default;

    /// <summary> S...16, A...8, B...4, C...2, D...1 </summary>
    private static int _clearRank = 0;

    public static int ClearRank { get => _clearRank; set => _clearRank = value; }

    public static int RankS = 16;
    public static int RankA = 8;
    public static int RankB = 4;
    public static int RankC = 2;
    public static int RankD = 1;

    private void Start()
    {
        _clearRank = 1;

        bool isSetting = false;
        int[] ranks = new int[5] { RankS, RankA, RankB, RankC, RankD };

        for (int i = 0; i < _state.Length; i++)
        {
            //現在の進行状況を反映
            if ((_clearRank & ranks[i]) == 1)
            {
                _state[i] = ClearState.Cleared;
            }
            else
            {
                _state[i] = ClearState.NotOpened;
            }

            var rank = 
                gameObject.transform.GetChild(i).
                gameObject.GetComponent<Button>();

            _buttons[i] = rank;

            //指定のランクステージが挑戦可能なら
            if (_state[i] != ClearState.NotOpened)
            {
                _indexes[i] = rank.GetComponent<MouseButton>().Index;
                if (!isSetting)
                {
                    _index = _indexes[i];
                    isSetting = true;
                }
            }

            //指定のランクステージが挑戦不可なら
            if (_state[i] == ClearState.NotOpened)
            {
                rank.GetComponent<Image>().color = Color.cyan;
                rank.GetComponent<MouseButton>().enabled = false;
                rank.interactable = false;
            }
            else
            {
                //クリア済だったら
                if (_state[i] == ClearState.Cleared)
                {
                    rank.gameObject.transform.GetChild(0).
                        gameObject.GetComponent<Text>().text += " [ Cleared!! ]";
                }
            }
        }

        _beforeIndex = _index;
        _startIndex = _index;

        _selectable = _buttons[_startIndex];
        _selectable.Select();

        ImageSwitch();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.UpArrow))
        {
            UIMove(true);
        }
        else if (Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.DownArrow))
        {
            UIMove(false);
        }
    }

    /// <summary> クリアしたステージ(ランク)を保存する </summary>
    public static void SaveStage(int stage)
    {
        _clearRank |= stage;
    }

    private void UIMove(bool num)
    {
        if (num)
        {
            _index--;
            if (_index < _startIndex)
            {
                _index = _buttons.Length - 1;
            }
        }
        else
        {
            _index++;
            if (_index >= _buttons.Length)
            {
                _index = _startIndex;
            }
        }

        _selectable = _buttons[_index];
        _selectable.Select();

        ImageSwitch();
    }

    private void UIMove(int num)
    {
        _beforeIndex = _index;
        _index = _indexes[num];

        _selectable = _buttons[_index];
        _selectable.Select();

        ImageSwitch();
    }

    private void ImageSwitch()
    {
        _stageImage.sprite = _images[_index];

        if (_state[_index] == ClearState.Challengeable)
        {
            _stageImage.color = Color.black;
        }
        else if (_state[_index] == ClearState.Cleared)
        {
            _stageImage.color = Color.white;
        }
    }

    public void UIMoveByMouse(int index)
    {
        _beforeIndex = _index;

        //条件分岐の判定順の都合上、以下のように書いている
        if (index == _buttons.Length - 1 && _beforeIndex == _startIndex)
        {
            UIMove(true);
        }
        else if (index == _startIndex && _beforeIndex == _buttons.Length - 1)
        {
            UIMove(false);
        }

        else if (index < _beforeIndex)
        {
            UIMove(index);
        }
        else if (index > _beforeIndex)
        {
            UIMove(index);
        }
    }
}

public enum ClearState
{
    /// <summary> 挑戦不可 </summary>
    NotOpened,
    /// <summary> 挑戦可 && 未クリア </summary>
    Challengeable,
    /// <summary> クリア済 </summary>
    Cleared,
}

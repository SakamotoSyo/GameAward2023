using UnityEngine;
using UnityEngine.UI;

public class ResultView : MonoBehaviour
{
    [SerializeField] private Text _oreName = default;
    [SerializeField] private Text _status = default;
    [SerializeField] private Text _flavorText = default;

    private OreSelect[] _ores = new OreSelect[3];

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _ores[i] = transform.GetChild(i).GetComponent<OreSelect>();
        }
    }

    public void ViewStatus(int num)
    {
        for (int i = 0; i < _ores.Length; i++)
        {
            _ores[i].gameObject.GetComponent<Image>().color = Color.white;
        }
        _ores[num].gameObject.GetComponent<Image>().color = Color.yellow;

        _oreName.text = "Name : " + _ores[num].transform.GetChild(0).GetComponent<Text>().text;
        _status.text = _ores[num].Parameter;
        _flavorText.text = _ores[num].FlavorText;
    }

    public void ParamSetting()
    {
        for (int i = 0; i < _ores.Length; i++)
        {
            if (!(_ores[i].gameObject.GetComponent<Image>().color == Color.yellow))
            {
                _ores[i].gameObject.SetActive(false);
            }
        }
    }

    public void ResetSelect()
    {
        for (int i = 0; i < _ores.Length; i++)
        {
            _ores[i].gameObject.SetActive(true);
            _ores[i].gameObject.GetComponent<Image>().color = Color.white;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleSelect : MonoBehaviour
{
    [SerializeField] private Text _infoText = default;
    [SerializeField] private Transform[] _battles = new Transform[4];

    private int _index = 0;

    private void Start()
    {
        MovePos(0);
    }

    public void MovePos(int num)
    {
        int n = Mathf.Abs(_index - num);

        for (int i = 0; i < _battles.Length; i++)
        {
            if (i != num)
            {
                if (_index < num)
                {
                    _battles[i].transform.DOLocalMoveX(
                        _battles[i].transform.localPosition.x + (-430f * n), 1f);
                }
                else if (_index > num)
                {
                    _battles[i].transform.DOLocalMoveX(
                        _battles[i].transform.localPosition.x + (430f * n), 1f);
                }

                _battles[i].GetChild(0).GetComponent<Image>().color = Color.white;
                for (int j = 1; j < _battles[i].childCount; j++)
                {
                    _battles[i].GetChild(j).gameObject.SetActive(false);
                }
            }
            else
            {
                _battles[i].transform.DOLocalMoveX(-500f, 1f);

                _battles[i].GetChild(0).GetComponent<Image>().color = Color.yellow;
                for (int j = 1; j < _battles[i].childCount; j++)
                {
                    _battles[i].GetChild(j).gameObject.SetActive(true);
                }
            }
        }
        _index = num;
        StageInfo(num);
    }

    private void StageInfo(int num)
    {
        //指定したステージの敵の情報等を反映させる
        var events = _battles[num].GetChild(0).GetComponent<ClickEvent>();
        _infoText.text = events.Info;
    }
}

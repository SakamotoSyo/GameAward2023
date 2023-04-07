using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary> ランクのText表示 </summary>
public class RankScene : MonoBehaviour
{
    [Tooltip("DOTweenの再生速度")]
    [Range(0.2f, 2f)]
    [SerializeField] private float _playSpeed = 1f;
    [SerializeField] private Text _rankText = default;
    [SerializeField] private Text _infoText = default;
    [SerializeField] private Color _selecting = Color.white;
    [SerializeField] private Transform[] _battles = new Transform[4];

    private int _index = 0;

    public static string Rank = "";
    public static Ranks Challenge = Ranks.None;

    private void Start()
    {
        if (Rank == "")
        {
            Rank = "Rank : XXX";
            Challenge = Ranks.None;
        }
        _rankText.text = Rank;

        MovePos(0);
        Debug.Log(Challenge + " に挑戦します");
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
                        _battles[i].transform.localPosition.x + (-600f * n), _playSpeed);
                }
                else if (_index > num)
                {
                    _battles[i].transform.DOLocalMoveX(
                        _battles[i].transform.localPosition.x + (600f * n), _playSpeed);
                }

                _battles[i].GetComponent<Image>().color = Color.white;
            }
            else
            {
                _battles[i].transform.DOLocalMoveX(-500f, _playSpeed);

                _battles[i].GetComponent<Image>().color = _selecting;
            }
        }
        _index = num;
        StageInfo(num);
    }

    private void StageInfo(int num)
    {
        //指定したステージの敵の情報等を反映させる
        var events = _battles[num].GetComponent<ClickEvent>();
        _infoText.text = events.Info;
    }
}

public enum Ranks
{
    None,
    RankS,
    RankA,
    RankB,
    RankC,
    RankD,
}

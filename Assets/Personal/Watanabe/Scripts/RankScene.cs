using UnityEngine;
using UnityEngine.UI;

/// <summary> ランクのText表示 </summary>
public class RankScene : MonoBehaviour
{
    [SerializeField] private Text _rankText = default;

    public static string Rank = "";

    private void Start()
    {
        if (Rank == "")
        {
            Rank = "Rank : XXX";
        }

        _rankText.text = Rank;
    }
}

using UnityEngine;
using UnityEngine.UI;

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

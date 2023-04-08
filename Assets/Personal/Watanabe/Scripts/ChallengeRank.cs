using UnityEngine;

public class ChallengeRank : MonoBehaviour
{
    [SerializeField] private Ranks _challenge = Ranks.None;

    public void SetRank()
    {
        RankScene.Challenge = _challenge;
    }
}

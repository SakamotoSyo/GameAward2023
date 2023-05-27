using UnityEngine;

public class FinishedAnim : MonoBehaviour
{
    [SerializeField] private GameObject _button = default;

    public void FinishedSetLogo()
    {
        _button.SetActive(true);
    }
}

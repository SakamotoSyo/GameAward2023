using CriWare;
using UnityEngine;
using UnityEngine.Events;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPlayAnim = default;
    [SerializeField] private CriAtomSource _atomSource = default;

    private void Start()
    {
        _atomSource.volume = 0.5f;
        SoundManager.Instance.CriAtomBGMPlay("BGM_Title");
    }

    public void PlayAnim()
    {
        _onPlayAnim?.Invoke();
        _atomSource.volume = 1f;
    }
}

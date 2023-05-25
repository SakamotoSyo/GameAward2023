using UnityEngine;
using UnityEngine.UI;

public class SelectWeapon : MonoBehaviour
{
    [SerializeField] private Image _outLineImage = default;

    private bool _isActive = false;

    private void Start()
    {
        _outLineImage.gameObject.SetActive(false);
        _isActive = false;
    }

    public void SelectThis()
    {
        _outLineImage.gameObject.SetActive(!_isActive);

        _isActive = _outLineImage.gameObject.activeSelf;
    }
}

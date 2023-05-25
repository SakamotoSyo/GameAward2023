using UnityEngine;
using UnityEngine.UI;

public class SelectWeapon : MonoBehaviour
{
    [SerializeField] private Image[] _outLineImage = new Image[4];

    private bool[] _isActive = new bool[4];

    private void Start()
    {
        for (int i = 0; i < 4 ; i++)
        {
            _outLineImage[i].gameObject.SetActive(false);
            _isActive[i] = false;
        }
    }

    public void SelectThis(int index)
    {
        for (int i = 0; i < 4; i++)
        {
            _outLineImage[i].gameObject.SetActive(false);
            _isActive[i] = false;
        }

        _outLineImage[index].gameObject.SetActive(!_isActive[index]);

        _isActive[index] = _outLineImage[index].gameObject.activeSelf;
    }
}

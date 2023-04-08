using UnityEngine;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    [SerializeField] private Button[] _weapons = new Button[4];
    [SerializeField] private bool[] _isSelect = new bool[4];

    private void Start()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            if (!_isSelect[i])
            {
                _weapons[i].GetComponent<Image>().color = Color.gray;
                _weapons[i].interactable = false;
            }
        }
    }
}

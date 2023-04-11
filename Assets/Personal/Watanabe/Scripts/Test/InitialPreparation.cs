using UnityEngine;
using UnityEngine.UI;

public class InitialPreparation : MonoBehaviour
{
    [SerializeField] private InitialWeaponSelect[] _weapons = new InitialWeaponSelect[4];

    [SerializeField] private Text _weaponName = default;
    [SerializeField] private Text _parameters = default;
    [SerializeField] private Text _flavorText = default;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ViewData(int num)
    {
        var events = _weapons[num].GetComponent<InitialWeaponSelect>();

        events.gameObject.GetComponent<Image>().color = Color.cyan;

        _weaponName.text = "武器名 : " + events.WeaponType.ToString();
        _parameters.text = events.Parameter;
        _flavorText.text = events.FlavorText;
    }
}

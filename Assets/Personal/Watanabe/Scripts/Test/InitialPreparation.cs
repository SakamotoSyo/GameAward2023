using System;
using UnityEngine;
using UnityEngine.UI;

public class InitialPreparation : MonoBehaviour
{
    [SerializeField] private Image[] _weaponHoldings = new Image[4];

    [SerializeField] private Text _weaponName = default;
    [SerializeField] private Text _parameters = default;
    [SerializeField] private Text _flavorText = default;
    [SerializeField] private Sprite _initialImage = default;

    /// <summary> クリックする画像(各武器) </summary>
    private InitialWeaponSelect[] _weapons = new InitialWeaponSelect[4];
    /// <summary> 画面に反映させる武器の画像 </summary>
    private Sprite[] _weaponImages = new Sprite[4];
    /// <summary> 手持ちの武器の種類 </summary>
    private WeaponType[] _holdings = new WeaponType[4];
    /// <summary> 現在選択中の武器 </summary>
    private WeaponType _selectedWeapon = default;

    private void Start()
    {
        Array.ForEach(_weaponHoldings, i => i.sprite = _initialImage);

        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i] = gameObject.transform.GetChild(i).GetComponent<InitialWeaponSelect>();
            _weaponImages[i] = _weapons[i].gameObject.GetComponent<Image>().sprite;
        }

        ViewData(0);
    }

    public void ViewData(int num)
    {
        Array.ForEach(_weapons, n => n.GetComponent<Image>().color = Color.white);

        var events = _weapons[num].GetComponent<InitialWeaponSelect>();

        events.gameObject.GetComponent<Image>().color = Color.yellow;

        _weaponName.text = "武器名 : " + events.Type.ToString();
        _parameters.text = events.Parameter;
        _flavorText.text = events.FlavorText;

        _selectedWeapon = events.Type;
    }

    public void SetWeapon()
    {
        for (int i = 0; i < _weaponHoldings.Length; i++)
        {
            if (_weaponHoldings[i].sprite == _initialImage)
            {
                _weaponHoldings[i].sprite = _weaponImages[(int)_selectedWeapon];
                _holdings[i] = _selectedWeapon;
                break;
            }
        }
    }

    public void ResetHolder()
    {
        Array.ForEach(_weaponHoldings, i => i.sprite = _initialImage);
    }
}

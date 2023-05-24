using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Text _currentHpText;
    [SerializeField] private Text _maxMpText;
    [SerializeField] private Image _currentHpImage;
    private float _maxHp;
    private float _currentHp;

    public void SetCurrentHp(float num) 
    {
        _currentHpText.text = num.ToString();
        _currentHp = num;
        AdjustmentHpBar();
        Debug.Log($"åªç›ÇÃHpÇÕ{num}");
    }

    public void SetMaxHp(float num) 
    {
        _maxMpText.text = num.ToString();
        _maxHp = num;
        AdjustmentHpBar();
    }

    private void AdjustmentHpBar() 
    {
        _currentHpImage.fillAmount = _currentHp / _maxHp;
    }

    public void ChangeWeaponIcon(WeaponType weaponType) 
    {

    }
}
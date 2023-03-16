using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Text _currentHpText;
    [SerializeField] private Text _maxMpText;

    public void SetCurrentHp(float num) 
    {
        _currentHpText.text = num.ToString();
    }

    public void SetMaxHp(float num) 
    {
        _maxMpText.text = num.ToString();
    }
}

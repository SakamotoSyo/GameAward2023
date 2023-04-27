using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemy : MonoBehaviour
{
    private static Image _selectedEnemy = default;

    private void Start()
    {
        _selectedEnemy = GetComponent<Image>();
    }

    private void Update()
    {
        
    }

    public static void SetImage(Sprite sprite)
    {
        _selectedEnemy.sprite = sprite;
    }
}

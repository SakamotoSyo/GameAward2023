using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemy : MonoBehaviour
{
    private static Button _decideButton = default;

    private static Image _selectedEnemy = default;

    private void Start()
    {
        _decideButton = GameObject.Find("Decide").GetComponent<Button>();
        _decideButton.interactable = false;

        _selectedEnemy = GetComponent<Image>();
    }

    private void Update()
    {
        
    }

    public static void SetImage(Sprite sprite, bool isBoss)
    {
        _selectedEnemy.sprite = sprite;
        _selectedEnemy.color = isBoss ? Color.black : Color.white;

        _decideButton.interactable = true;
    }
}

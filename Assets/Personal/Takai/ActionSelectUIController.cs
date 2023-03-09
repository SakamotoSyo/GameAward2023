using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// TODO ¡Œã‚Ìs“®‚ªŒˆ’èŸ‘æ‚Å•ÏX‚·‚é
/// </summary>
public class ActionSelectUIController : MonoBehaviour
{
    [SerializeField] private Button _attackButton;
    [SerializeField] private Button _somethingButton1;
    [SerializeField] private Button _somethingButton2;
    [SerializeField] private Button _somethingButton3;

    GameObject _savePanel;
    public void OnSelectAction(GameObject panel)
    {
        if (_savePanel) { _savePanel.SetActive(false); }

        panel.SetActive(true);
        _savePanel = panel;
    }
}



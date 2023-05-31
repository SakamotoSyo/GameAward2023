using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimation : MonoBehaviour
{

    [SerializeField]
    private GameObject _tutorialPanel;

    [SerializeField]
    private GameObject _onTutorialPanel;

    public void OnNextTutorial()
    {
        _tutorialPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OffTutorial()
    {
        gameObject.SetActive(false);
        _tutorialPanel.SetActive(false);
    }

    public void OnTutorial()
    {
        _onTutorialPanel.SetActive(true);
    }
}

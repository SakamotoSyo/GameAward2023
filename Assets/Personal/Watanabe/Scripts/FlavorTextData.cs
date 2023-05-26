using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlavorTextData : MonoBehaviour
{
    [Multiline(5)]
    [SerializeField] private string _flavorText = "";

    public string flavorText => _flavorText;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}

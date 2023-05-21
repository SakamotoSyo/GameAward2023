using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClearEase : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(new Vector3(4.5f, 4.5f, 4.5f), 1f)
                 .SetEase(Ease.OutBack);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

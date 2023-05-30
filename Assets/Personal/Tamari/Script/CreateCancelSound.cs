using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateCancelSound : MonoBehaviour
{
    public void CancelSound()
    {
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Cancel");
    }
}

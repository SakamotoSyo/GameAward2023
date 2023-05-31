using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnterSound : MonoBehaviour
{
    public void EnterSound()
    {
        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Enter");
    }
}

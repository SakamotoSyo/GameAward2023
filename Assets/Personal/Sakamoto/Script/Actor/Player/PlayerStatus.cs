using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using VContainer.Unity;

public class PlayerStatus : ActorStatusBase, IPlayerStatus
{
    private PlayerStatus() 
    {
        Init();
    }

    public IStatusBase GetStatusBase()
    {
        return this;
    }

}

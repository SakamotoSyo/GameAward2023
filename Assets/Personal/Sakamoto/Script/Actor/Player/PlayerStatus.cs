using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerStatus : ActorStatusBase, IPlayerStatus
{
    public IStatusBase GetStatusBase()
    {
        return this;
    }
}

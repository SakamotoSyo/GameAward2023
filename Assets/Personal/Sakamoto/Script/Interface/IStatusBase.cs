using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
public interface IStatusBase
{
    public IReactiveProperty<float> GetMaxHpOb();
    public IReactiveProperty<float> GetCurrentHpOb();
    public void AddDamage(float damage);
    public bool DownJudge(float damage);
}

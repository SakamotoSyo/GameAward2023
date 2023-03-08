using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public interface ISkill
{
    /// <summary>
    /// スキルをスタートさせる
    /// </summary>
    /// <returns></returns>
    public UniTask StartSkill();
    /// <summary>
    /// スキルを使った結果を返す
    /// </summary>
    /// <returns></returns>
    public float SkillResult();
    /// <summary>
    /// スキルが終わった後に呼ばれる関数
    /// 外から呼ばなくてよい
    /// </summary>
    public void SkillEnd();
}

using Cysharp.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public class Sword : ISkill
{
    [SerializeField] private float _attack = 1f;
    [SerializeField] private float _critical = 1f;

    public string SkillEffectEnemyAnim()
    {
        throw new System.NotImplementedException();
    }

    public string SkillEffectAnimName()
    {
        throw new System.NotImplementedException();
    }

    public void SkillEnd()
    {
        throw new System.NotImplementedException();
    }

    public float SkillResult()
    {
        throw new System.NotImplementedException();
    }

    public UniTask StartSkill()
    {
        throw new System.NotImplementedException();
    }
}

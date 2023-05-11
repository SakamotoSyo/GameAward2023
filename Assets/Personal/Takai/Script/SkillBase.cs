using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public abstract class SkillBase : MonoBehaviour
{
    public string SkillName { get; protected set; }
    public int Damage { get; protected set; }
    public  WeaponType Weapon { get; protected set; }
    public SkillType Type { get; protected set; }
    public string FlavorText { get; protected set; }
    public bool IsUseCheck { get; protected set; }  
    
    public abstract UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType);
    protected abstract void SkillEffect();
    public abstract bool TurnEnd();
    public abstract void BattleFinish();
}

public enum SkillType
{
    Skill,
    Special,
}
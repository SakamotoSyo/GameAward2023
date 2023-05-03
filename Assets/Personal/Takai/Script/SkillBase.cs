using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public abstract class SkillBase : MonoBehaviour
{
    public abstract string SkillName { get; protected set; }
    public abstract int Damage { get; protected set; }
    public abstract WeaponType Weapon { get; protected set; }
    public abstract SkillType Type { get; protected set; }
    public abstract string FlavorText { get; protected set; }

    public abstract UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon,ActorAttackType actorType);
    protected abstract void SkillEffect();
    public abstract void TurnEnd();
    public abstract void BattleFinish();
}

public enum SkillType
{
    Skill,
    Special,
}
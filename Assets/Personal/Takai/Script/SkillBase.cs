using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

public abstract class SkillBase : MonoBehaviour
{
    public string SkillName { get; protected set; }
    public int Damage { get; protected set; }
    public int RequiredPoint { get; protected set; }
    public WeaponType Weapon { get; protected set; }
    public SkillType Type { get; protected set; }
    public string FlavorText { get; protected set; }

    public abstract bool IsUseCheck(ActorGenerator actor);
    public abstract UniTask UseSkill(PlayerController player, EnemyController enemy, ActorAttackType actorType);

    /// <summary>
    /// Skill効果発動中に外部からSkillを呼び出す用の関数
    /// </summary>
    /// <param name="attackType"></param>
    /// <returns></returns>
    public virtual async UniTask<bool> InEffectSkill(ActorAttackType attackType)
    {
        return default;
    }

    protected abstract void SkillEffect();
    /// <summary>
    /// falseにするとUsePoolから削除される
    /// </summary>
    /// <returns></returns>
    public abstract bool TurnEnd();
    public abstract void BattleFinish();
}

public enum SkillType
{
    Skill,
    Special,
    Epic,
}
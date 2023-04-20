using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

public class EnemyAttack
{
    private float _offensivePower;
    private Animator _anim;
    private EnemyAttackType _attackType = new();

    public void Init(float offensivePower, Animator anim)
    {
        _offensivePower = offensivePower;
        _anim = anim;
    }

    /// <summary>
    /// ‹Z‚ğƒ‰ƒ“ƒ_ƒ€‚Å‘I‘ğ
    /// </summary>
    public void SelectAttack(PlayerController playerController)
    {
        int r = Random.Range(0, Enum.GetNames(typeof(EnemyAttackType)).Length);
        _attackType = (EnemyAttackType)r;

        if(_attackType == EnemyAttackType.Normal)
        {
            NormalAttack(playerController);
        }
        else if(_attackType == EnemyAttackType.Skill)
        {
            SkillAttack(playerController);
        }
        else
        {
            SpecialAttack(playerController);
        }
    }

    /// <summary>
    /// UŒ‚
    /// </summary>
    public async UniTask NormalAttack(PlayerController playerController)
    {
        Debug.Log("normal");
        playerController.AddDamage(_offensivePower);
       // await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }

    /// <summary>
    /// ƒXƒLƒ‹UŒ‚
    /// </summary>
    private async UniTask SkillAttack(PlayerController playerController)
    {
        int r = Random.Range(0, 2);
        if(r == 0)
        {
            Debug.Log("skill1");
            playerController.AddDamage(_offensivePower);
            //await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        }
        else
        {
            Debug.Log("skill2");
            playerController.AddDamage(_offensivePower);
            //await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        }
    }

    /// <summary>
    /// •KE‹Z
    /// </summary>
    private async UniTask SpecialAttack(PlayerController playerController)
    {
        Debug.Log("special");
        playerController.AddDamage(_offensivePower);
        //await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }
}

public enum EnemyAttackType
{
    Normal,
    Skill,
    Special,
}

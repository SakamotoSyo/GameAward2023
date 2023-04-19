using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyAttack
{
    private float _offensivePower;
    private Animator _anim;

    public void Init(float offensivePower, Animator anim)
    {
        _offensivePower = offensivePower;
        _anim = anim;
    }

    /// <summary>
    /// çUåÇ
    /// </summary>
    public async UniTask NormalAttack(PlayerController playerController)
    {
        playerController.AddDamage(_offensivePower);
        await UniTask.WaitUntil(() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
    }
}

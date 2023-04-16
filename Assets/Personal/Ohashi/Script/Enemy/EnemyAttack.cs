using UnityEngine;

public class EnemyAttack
{
    private float _offensivePower;

    public void Init(float offensivePower)
    {
        _offensivePower = offensivePower;
    }

    /// <summary>
    /// çUåÇ
    /// </summary>
    public void Attack(PlayerController playerController)
    {
        playerController.AddDamage(_offensivePower);
    }
}

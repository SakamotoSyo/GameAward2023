using UnityEngine;

[System.Serializable]
public class EnemyAttack
{
    [SerializeField]
    private float _attackPower = 10f;

    private Animator _animator;

    public void Init(Animator animator)
    {
        _animator = animator;
    }

    /// <summary>
    /// çUåÇ
    /// </summary>
    public void Attack(PlayerController playerController)
    {
        _animator.Play("Attack");
        playerController.AddDamage(_attackPower);
    }
}

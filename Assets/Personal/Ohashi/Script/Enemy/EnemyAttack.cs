using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using Random = UnityEngine.Random;

public class EnemyAttack
{
    private float _offensivePower;

    private Animator _anim;

    private EquipEnemyWeapon _equipWepon;

    private SkillDataManagement _skillDataManagement;

    private int[] _skillArray = new int[6] { 0, 0, 0, 0, 1, 1 };

    public void Init(EquipEnemyWeapon equipWepon, Animator anim)
    {
        _equipWepon = equipWepon;
        _anim = anim;
        _skillDataManagement = GameObject.Find("SkillDataBase").GetComponent<SkillDataManagement>();
    }

    /// <summary>
    /// ãZÇÉâÉìÉ_ÉÄÇ≈ëIë
    /// </summary>
    public void SelectAttack(PlayerController playerController)
    {
        int  index= Random.Range(0, _skillArray.Length);

        if (_equipWepon.CurrentDurable.Value <= _equipWepon.CurrentDurable.Value / 2)
        {
            SpecialAttack(playerController);
        }

        if (_skillArray[index] == 0)
        {
            NormalAttack(playerController);
        }
        else
        {
            SkillAttack(playerController);
        }
    }

    /// <summary>
    /// çUåÇ
    /// </summary>
    public async UniTask NormalAttack(PlayerController playerController)
    {
        Debug.Log("normal");
        playerController.AddDamage(_equipWepon.CurrentOffensivePower);
    }

    /// <summary>
    /// ÉXÉLÉãçUåÇ
    /// </summary>
    private async UniTask SkillAttack(PlayerController playerController)
    {
        int r = Random.Range(0, 2);
        if(r == 0)
        {
            Debug.Log("skill1");
            await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, _equipWepon.WeaponSkill.WeaponSkillArray[0]);
            playerController.AddDamage(_equipWepon.CurrentOffensivePower);
        }
        else
        {
            Debug.Log("skill2");
            await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, _equipWepon.WeaponSkill.WeaponSkillArray[1]);
            playerController.AddDamage(_equipWepon.CurrentOffensivePower);
        }
    }

    /// <summary>
    /// ïKéEãZ
    /// </summary>
    private async UniTask SpecialAttack(PlayerController playerController)
    {
        Debug.Log("special");
        await _skillDataManagement.OnSkillUse(ActorAttackType.Enemy, _equipWepon.WeaponSkill.SpecialAttack);
        playerController.AddDamage(_equipWepon.CurrentOffensivePower);
    }
}
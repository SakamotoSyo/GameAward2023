using UnityEngine;
using DG.Tweening;

public class BattleSelectUI : MonoBehaviour
{
    [SerializeField] private Transform[] _actionUi = new Transform[4];

    [SerializeField] private GameObject _commandUI = default;
    [SerializeField] private GameObject _infoUI = default;

    [SerializeField] private ActorGenerator _generator = default;
    [SerializeField] private BattleChangeWeapon _battleChangeWeaponCs = default;
    [SerializeField] private SkillDataManagement _skillDataManagement = default;

    private int _index = 0;
    private PlayerController _playerController = default;
    private PlayerStatus _playerStatus = default;
    private EnemyController _enemyController = default;

    private void Start()
    {
        _playerController = _generator.PlayerController;
        _playerStatus = _playerController.PlayerStatus;
        _enemyController = _generator.EnemyController;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            BattleSelect(false);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            BattleSelect(true);
        }

        Attack();
    }

    private void BattleSelect(bool rot)
    {
        _actionUi[_index].DOScale(new Vector3(2f, 2f, 1f), 0.2f);

        if (rot)
        {
            _index = (_index + 1) % _actionUi.Length;
        }
        else
        {
            _index = _index == 0 ? _actionUi.Length - 1 : _index - 1;
            _index %= _actionUi.Length;
        }
        _actionUi[_index].DOScale(new Vector3(2.5f, 2.5f, 1f), 0.2f);
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _playerStatus.ChackAnomaly())
        {
            var num = _index % _actionUi.Length;
            if (num < 0)
            {
                num += _actionUi.Length;
            }


            if (_actionUi[num] == _actionUi[0])
            {
                Debug.Log("attack");
                _infoUI.SetActive(false);
                _enemyController.AddDamage((int)_playerController.Attack(PlayerAttackType.ConventionalAttack));
            }
            else if (_actionUi[num] == _actionUi[1])
            {
                //skill 1

                _infoUI.SetActive(true);
                SkillBase skill1 = _playerController.PlayerSkill.PlayerSkillArray[0];
                if (skill1)
                {
                    _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill1.name);
                }
            }
            else if (_actionUi[num] == _actionUi[2])
            {
                //必殺技
                Debug.Log("必殺技");
                _infoUI.SetActive(false);
            }
            else if (_actionUi[num] == _actionUi[3])
            {
                //skill 2

                _infoUI.SetActive(true);
                SkillBase skill2 = _playerController.PlayerSkill.PlayerSkillArray[1];
                if (skill2)
                {
                    _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill2.name);
                }
            }

            _commandUI.SetActive(false);
            _battleChangeWeaponCs.ChangeWeaponUiOpen();
            //TODO:遷移の機能を別の場所に移す
            //_battleStateController.ActorStateEnd();
        }
    }
}

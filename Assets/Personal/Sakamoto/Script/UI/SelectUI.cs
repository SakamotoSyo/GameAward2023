using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    [Tooltip("行動を決めるUI")]
    [SerializeField] private Transform[] _actionUi = new Transform[4];
    [SerializeField] private Transform[] _actionUiPos = new Transform[4];
    [SerializeField] private Text[] _skillText = new Text[3];
    [SerializeField] private GameObject _commandUI;
    [SerializeField] private GameObject _infoUI;
    [SerializeField] private ActorGenerator _generator;
    [SerializeField] private BattleStateController _battleStateController;
    [SerializeField] private BattleChangeWeapon _battleChangeWeaponCs;
    [SerializeField] private SkillDataManagement _skillDataManagement;
    [SerializeField] private int _lotateNum;

    private WeaponStatus _weaponStatus;
    private PlayerController _playerController;
    private PlayerStatus _playerStatus;
    private EnemyController _enemyController;

    private void Start()
    {
        _playerController = _generator.PlayerController;
        _playerStatus = _playerController.PlayerStatus;
        _enemyController = _generator.EnemyController;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Left"))
        {
            UiMove(true);
            _lotateNum = (_lotateNum + 1) % _actionUi.Length;
            SkillInfo();
        }
        else if (Input.GetButtonDown("Right"))
        {
            UiMove(false);
            _lotateNum = (_lotateNum - 1) % _actionUi.Length;
            SkillInfo();
        }

        Attack();
    }

    private void UiMove(bool num)
    {

        for (int i = 0; i < _actionUi.Length; i++)
        {
            int j = i;
            var nextMoveNum = 0;
            if (num)
            {
                //時計回りの処理
                nextMoveNum = ((i + 1) + _lotateNum) % _actionUi.Length;
            }
            else
            {
                //半時計回り
                nextMoveNum = ((i - 1) + _lotateNum) % _actionUi.Length;
            }

            if (nextMoveNum < 0)
            {
                nextMoveNum += _actionUi.Length;
            }

            DOTween.To(() => _actionUi[j].transform.localPosition,
                       x => _actionUi[j].transform.localPosition = x,
                     _actionUiPos[nextMoveNum].transform.localPosition, 0.5f);

            DOTween.To(() => _actionUi[j].transform.localScale,
                       x => _actionUi[j].transform.localScale = x,
                       _actionUiPos[nextMoveNum].transform.localScale, 0.5f);

            if (i == 0)
            {
                _actionUi[nextMoveNum].SetAsLastSibling();
            }
        }

     
    }

    public void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _playerStatus.ChackAnomaly())
        {
            var num = _lotateNum % _actionUi.Length;
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
                Debug.Log("必殺技");
                _infoUI.SetActive(false);
            }
            else if (_actionUi[num] == _actionUi[2])
            {
                _infoUI.SetActive(true);
                SkillBase skill1 = _playerController.PlayerSkill.PlayerSkillArray[0];
                if (skill1) 
                {
                    _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill1.name);
                }
            }
            else if (_actionUi[num] == _actionUi[3])
            {
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

    private void SkillInfo() 
    {
        var currentUi = _lotateNum % _actionUi.Length;
        if (currentUi < 0)
        {
            currentUi += _actionUi.Length;
        }

        if (_actionUi[currentUi] == _actionUi[2])
        {
            _infoUI.SetActive(true);
            SkillBase skill1 = _playerController.PlayerSkill.PlayerSkillArray[0];
            if (skill1 != null)
            {
                _skillText[0].text = skill1.name;
                _skillText[1].text = skill1.Damage.ToString();
                _skillText[2].text = skill1.FlavorText;
                Debug.Log("テキストです");
            }
            else
            {
                Debug.Log("nullデス");
            }

        }
        else if (_actionUi[currentUi] == _actionUi[3])
        {
            _infoUI.SetActive(true);
            SkillBase skill2 = _playerController.PlayerSkill.PlayerSkillArray[1];
            if (skill2 != null)
            {
                _skillText[0].text = skill2.name;
                _skillText[1].text = skill2.Damage.ToString();
                _skillText[2].text = skill2.FlavorText;
            }
            else
            {

            }

        }
        else
        {
            _infoUI.SetActive(false);
        }
    }

    public void StartPlayerTurn()
    {
        var playerSkill = _playerController.PlayerSkill;
        _skillText[0].text = playerSkill.SpecialAttack.SkillName;
        for (int i = 1; i < playerSkill.PlayerSkillArray.Length + 1; i++)
        {
            _skillText[i].text = playerSkill.PlayerSkillArray[i - 1].SkillName;
        }

        _commandUI.SetActive(true);
    }
}

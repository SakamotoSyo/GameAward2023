using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleSelectUI : MonoBehaviour
{
    [SerializeField] private Transform[] _actionUi = new Transform[4];
    [SerializeField] private Text[] _skillText = new Text[3];

    [SerializeField] private GameObject _commandUI = default;
    [SerializeField] private GameObject _infoUI = default;

    [SerializeField] private ActorGenerator _generator = default;
    [SerializeField] private BattleChangeWeapon _battleChangeWeaponCs = default;
    [SerializeField] private SkillDataManagement _skillDataManagement = default;
    [SerializeField] private BattleStateController _battleStateController;

    private int _index = 0;
    private PlayerController _playerController = default;
    private PlayerStatus _playerStatus = default;
    private EnemyController _enemyController = default;

    public Transform[] ActionUI => _actionUi;

    private void Start()
    {
        _playerController = _generator.PlayerController;
        _playerStatus = _playerController.PlayerStatus;
        _enemyController = _generator.EnemyController;

        _actionUi[0].DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            BattleSelect(0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            BattleSelect(2);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            BattleSelect(3);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            BattleSelect(1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _playerStatus.ChackAnomaly())
        {
            _battleChangeWeaponCs.ChangeWeaponUiOpen();
        }
    }

    public void BattleSelect(int dir)
    {
        _actionUi[_index].DOScale(new Vector3(1f, 1f, 1f), 0.2f);

        if (_index != dir)
        {
            _index = dir;
        }
        _actionUi[_index].DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f);

        SkillInfo();
    }

    public void EffectCheck() 
    {

    }

    public async void AttackEvent()
    {
        if (_actionUi[_index] == _actionUi[0])
        {
            _enemyController.AddDamage((int)_playerController.Attack(PlayerAttackType.ConventionalAttack));
            _battleStateController.ActorStateEnd();
        }
        else if (_actionUi[_index] == _actionUi[1])
        {
            SkillBase skill1 = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[0];
            if (skill1 && skill1.IsUseCheck(_playerController))
            {
                await _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill1.name);
                _battleStateController.ActorStateEnd();
            }
        }
        else if (_actionUi[_index] == _actionUi[2])
        {
            SkillBase spcialSkill = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.SpecialAttack;
            if (spcialSkill && spcialSkill.IsUseCheck(_playerController))
            {
                await _skillDataManagement.OnSkillUse(ActorAttackType.Player, spcialSkill.name);
                _battleStateController.ActorStateEnd();
            }
        }
        else if (_actionUi[_index] == _actionUi[3])
        {
            SkillBase skill2 = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[1];
            if (skill2 && skill2.IsUseCheck(_playerController))
            {
                await _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill2.name);
                _battleStateController.ActorStateEnd();
            }
        }
    }

    private void SkillInfo()
    {
        var currentUi = _index % _actionUi.Length;
        if (currentUi < 0)
        {
            currentUi += _actionUi.Length;
        }

        if (_actionUi[currentUi] == _actionUi[1])
        {
            _infoUI.SetActive(true);
            SkillBase skill1 = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[0];
            if (skill1 != null)
            {
                _skillText[0].text = skill1.name;
                _skillText[1].text = skill1.Damage.ToString();
                _skillText[2].text = skill1.FlavorText;
                Debug.Log("テキストです");
            }
            else
            {
                _skillText[0].text = "NoSkill";
                _skillText[1].text = "";
                _skillText[2].text = "";
            }

        }
        else if (_actionUi[currentUi] == _actionUi[3])
        {
            _infoUI.SetActive(true);
            SkillBase skill2 = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[1];
            if (skill2 != null)
            {
                _skillText[0].text = skill2.name;
                _skillText[1].text = skill2.Damage.ToString();
                _skillText[2].text = skill2.FlavorText;
            }
            else
            {
                _skillText[0].text = "NoSkill";
                _skillText[1].text = "";
                _skillText[2].text = "";
            }

        }
        else if (_actionUi[currentUi] == _actionUi[2])
        {
            _infoUI.SetActive(true);
            SkillBase special = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.SpecialAttack;
            if (special != null)
            {
                _skillText[0].text = special.name;
                _skillText[1].text = special.Damage.ToString();
                _skillText[2].text = special.FlavorText;
            }
            else
            {
                _skillText[0].text = "NoSkill";
                _skillText[1].text = "";
                _skillText[2].text = "";
            }
        }
        else
        {
            _infoUI.SetActive(false);
        }
    }
}

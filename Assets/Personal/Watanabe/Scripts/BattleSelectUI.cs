using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class BattleSelectUI : MonoBehaviour
{
    [SerializeField] private Transform[] _actionUi = new Transform[4];
    [SerializeField] private Text[] _skillText = new Text[4];

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

    private bool[] _isAttackable = new bool[3];

    public Transform[] ActionUI => _actionUi;
    public bool[] IsAttackable => _isAttackable;

    private void Awake()
    {
        _skillDataManagement = GameObject.Find("SkillDataBase").GetComponent<SkillDataManagement>();
    }

    private void Start()
    {
        _playerController = _generator.PlayerController;
        _playerStatus = _playerController.PlayerStatus;
        _enemyController = _generator.EnemyController;
        _actionUi[0].DOScale(new Vector3(1.2f, 1.2f, 1f), 0.2f);
        UseSkillCheck();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            BattleSelect(0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (!_isAttackable[1])
            {
                return;
            }
            BattleSelect(2);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (!_isAttackable[2])
            {
                return;
            }
            BattleSelect(3);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (!_isAttackable[0])
            {
                return;
            }
            BattleSelect(1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && _playerStatus.ChackAnomaly())
        {
            if (_index == 0 || (_index != 0 && _isAttackable[_index]))
            {
                AttackEvent();
                //_battleChangeWeaponCs.ChangeWeaponUiOpen();
            }

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
        _commandUI.SetActive(false);
        _infoUI.SetActive(false);
        if (_actionUi[_index] == _actionUi[0])
        {
            await NormalAttack();
        }
        else if (_actionUi[_index] == _actionUi[1])
        {
            SkillBase skill1 = _skillDataManagement.SearchSkill(_playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[0]);
            if (skill1 && skill1.IsUseCheck(_generator))
            {
                _playerController.AddDamage(skill1.RequiredPoint, 0, true);
                await _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill1.SkillName);
            }
        }
        else if (_actionUi[_index] == _actionUi[2])
        {
            SkillBase spcialSkill = _skillDataManagement.SearchSkill(_playerController.PlayerStatus.EquipWeapon.WeaponSkill.SpecialAttack);
            if (spcialSkill && spcialSkill.IsUseCheck(_generator))
            {
                _playerController.AddDamage(spcialSkill.RequiredPoint, 0, true);
                await _skillDataManagement.OnSkillUse(ActorAttackType.Player, spcialSkill.SkillName);
            }
        }
        else if (_actionUi[_index] == _actionUi[3])
        {
            SkillBase skill2 = _skillDataManagement.SearchSkill(_playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[1]);
            if (skill2 && skill2.IsUseCheck(_generator))
            {
                _playerController.AddDamage(skill2.RequiredPoint, 0, true);
                await _skillDataManagement.OnSkillUse(ActorAttackType.Player, skill2.SkillName);
            }
        }

        bool result = await _enemyController.EnemyStatus.IsWeaponsAllBrek();
        if (result)
        {
            _battleStateController.ActorStateEnd();
        }
        else 
        {
            if (_playerStatus.EquipWeapon.IsEpicSkill2)
            {
                _battleStateController.ActorStateEnd();
            }
            else 
            {
                _battleChangeWeaponCs.ChangeWeaponUiOpen();
            }
            
        }
    }

    public async UniTask NormalAttack()
    {
        Debug.Log("normal");
        Debug.Log($"プレイヤーの武器のタイプは{_playerStatus.EquipWeapon.WeaponType}です");
        if (_playerStatus.EquipWeapon.WeaponType == WeaponType.GreatSword)
        {
            var skill = _skillDataManagement.SearchSkill("溜め斬り");
            _playerController.AddDamage(skill.RequiredPoint, 0, true);
            await _skillDataManagement.OnSkillUse(ActorAttackType.Player, "溜め斬り");
        }
        else if (_playerStatus.EquipWeapon.WeaponType == WeaponType.DualBlades)
        {
            Debug.Log("総研");
            var skill = _skillDataManagement.SearchSkill("連続斬り");
            _playerController.AddDamage(skill.RequiredPoint, 0, true);
            await _skillDataManagement.OnSkillUse(ActorAttackType.Player, "連続斬り");
        }
        else if (_playerStatus.EquipWeapon.WeaponType == WeaponType.Hammer)
        {
            var skill = _skillDataManagement.SearchSkill("全力打ち");
            _playerController.AddDamage(skill.RequiredPoint, 0, true);
            await _skillDataManagement.OnSkillUse(ActorAttackType.Player, "全力打ち");

        }
        else if (_playerStatus.EquipWeapon.WeaponType == WeaponType.Spear)
        {
            var skill = _skillDataManagement.SearchSkill("一閃");
            _playerController.AddDamage(skill.RequiredPoint, 0, true);
            await _skillDataManagement.OnSkillUse(ActorAttackType.Player, "一閃");
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
            SkillBase skill1 = _skillDataManagement.SearchSkill(_playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[0]);
            if (skill1 != null)
            {
                _skillText[0].text = skill1.SkillName;
                _skillText[1].text = skill1.Damage.ToString();
                _skillText[2].text = skill1.FlavorText;
                _skillText[3].text = skill1.RequiredPoint.ToString();
                Debug.Log("テキストです");
            }
            else
            {
                _skillText[0].text = "NoSkill";
                _skillText[1].text = "";
                _skillText[2].text = "";
                _skillText[3].text = "";

            }

        }
        else if (_actionUi[currentUi] == _actionUi[3])
        {
            _infoUI.SetActive(true);
            SkillBase skill2 = _skillDataManagement.SearchSkill(_playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[1]);
            if (skill2 != null)
            {
                _skillText[0].text = skill2.SkillName;
                _skillText[1].text = skill2.Damage.ToString();
                _skillText[2].text = skill2.FlavorText;
                _skillText[3].text = skill2.RequiredPoint.ToString();
            }
            else
            {
                _skillText[0].text = "NoSkill";
                _skillText[1].text = "";
                _skillText[2].text = "";
                _skillText[3].text = "";
            }

        }
        else if (_actionUi[currentUi] == _actionUi[2])
        {
            _infoUI.SetActive(true);
            SkillBase special = _skillDataManagement.SearchSkill(_playerController.PlayerStatus.EquipWeapon.WeaponSkill.SpecialAttack);
            if (special != null)
            {
                _skillText[0].text = special.SkillName;
                _skillText[1].text = special.Damage.ToString();
                _skillText[2].text = special.FlavorText;
                _skillText[3].text = special.RequiredPoint.ToString();
            }
            else
            {
                _skillText[0].text = "NoSkill";
                _skillText[1].text = "";
                _skillText[2].text = "";
                _skillText[3].text = "";  
            }
        }
        else
        {
            _infoUI.SetActive(false);
        }
    }

    public void UseSkillCheck() 
    {

        for (int i = 0; i < _isAttackable.Length; i++)
        {
            _isAttackable[i] = true;
        }

        var skillName1 = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[0];
        var skillName2 = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.WeaponSkillArray[1];
        var specialSkill = _playerController.PlayerStatus.EquipWeapon.WeaponSkill.SpecialAttack;
        if (skillName1 == null || _skillDataManagement.IsUseCheck(skillName1, _generator))
        {
            _isAttackable[0] = false;
            _actionUi[1].GetChild(0).GetComponent<Image>().color = Color.gray;
        }
        if (skillName2 == null || _skillDataManagement.IsUseCheck(skillName2, _generator))
        {
            _isAttackable[2] = false;
            _actionUi[3].GetChild(0).GetComponent<Image>().color = Color.gray;
        }
        if (specialSkill == null || _skillDataManagement.IsUseCheck(specialSkill, _generator))
        {
            _isAttackable[1] = false;
            _actionUi[2].GetComponent<Image>().color = Color.gray;
        }
    }
}

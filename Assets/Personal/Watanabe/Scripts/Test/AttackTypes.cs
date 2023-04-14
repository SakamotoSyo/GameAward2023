using UnityEngine;
using UnityEngine.UI;

public class AttackTypes : MonoBehaviour
{
    [SerializeField] private Strength[] _strengths = new Strength[4];
    [SerializeField] private Transform _switchWeapon = default;
    [SerializeField] private Image _currentWeapon = default;

    [SerializeField] private Text _name = default;
    [SerializeField] private Text _effect = default;

    private AttackStatus[] _status = new AttackStatus[4];

    private Image[] _weapons = new Image[4];
    private Image[] _attacks = new Image[4];
    private int _index = 0;
    private bool _isAttack = false;

    private void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            _status[i] = gameObject.transform.GetChild(i).GetComponent<AttackStatus>();
            _attacks[i] = gameObject.transform.GetChild(i).GetComponent<Image>();
        }

        for (int i = 0; i < _switchWeapon.childCount; i++)
        {
            _weapons[i] = _switchWeapon.GetChild(i).GetComponent<Image>();
        }
        ImageView(_index);
        _currentWeapon.sprite = _weapons[_index].sprite;
        _switchWeapon.gameObject.SetActive(false);

        _name.text = _attacks[0].gameObject.transform.GetChild(0).GetComponent<Text>().text;
        _effect.text = _status[0].Status;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _index--;
            if (_index < 0)
            {
                _index = _attacks.Length - 1;
            }
            ImageView (_index);
        }
        else if (Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            _index++;
            if (_index >= _attacks.Length)
            {
                _index = 0;
            }
            ImageView(_index);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            AttackTest(_strengths[_index]);
        }
    }

    private void ImageView(int index)
    {
        for (int i = 0; i < _attacks.Length; i++)
        {
            _weapons[i].color = Color.white;
            _attacks[i].color = Color.white;
        }
        _weapons[index].color = Color.yellow;
        _attacks[index].color = Color.yellow;

        if (!_isAttack)
        {
            _name.text = _attacks[index].gameObject.transform.GetChild(0).GetComponent<Text>().text;
            _effect.text = _status[index].Status;
        }
    }

    private void AttackTest(Strength strength)
    {
        if (!_isAttack)
        {
            //äeçUåÇÇåƒÇ‘(âº)
            switch (strength)
            {
                case Strength.Normal:
                    break;
                case Strength.SkillOne:
                    break;
                case Strength.SkillTwo:
                    break;
                case Strength.Deathblow:
                    break;
            }
            Debug.Log("çUåÇ " + strength);

            _switchWeapon.gameObject.SetActive(true);

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
            _isAttack = true;
        }
        else
        {
            _currentWeapon.sprite = _weapons[_index].sprite;
            _switchWeapon.gameObject.SetActive(false);

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
            _isAttack = false;
        }
    }
}

public enum Strength
{
    Normal,
    SkillOne,
    SkillTwo,
    Deathblow,
}

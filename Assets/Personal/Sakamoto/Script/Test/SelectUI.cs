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
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private int _lotateNum;
    [SerializeField] private GameObject _commandUI;

    private WeaponStatus _weaponStatus;
    private PlayerStatus _playerStatus;

    private void Start()
    {
        _playerStatus = _playerController.PlayerStatus;
    }

    private void Update()
    {

        if (Input.GetButtonDown("Left"))
        {
            UiMove(true);
            _lotateNum = (_lotateNum + 1) % 4;
        }
        else if (Input.GetButtonDown("Right"))
        {
            UiMove(false);
            _lotateNum = (_lotateNum - 1) % 4;
        }

        TestAttack();
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

    public void TestAttack() 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            var num = _lotateNum % _actionUi.Length;
            if (num < 0)
            {
                num += _actionUi.Length;
            }


            if (_actionUi[num] == _actionUi[0]) 
            {
                Debug.Log("attack");
                GameObject.Find("Weapon").GetComponent<WeaponStatus>().SelectType(0);
            }
            else if (_actionUi[num] == _actionUi[1])
            {
                Debug.Log("必殺技");
            }
            else if (_actionUi[num] == _actionUi[2])
            {
                Debug.Log("none 1");
                //未定
            }
            else
            {
                Debug.Log("none 2");
                //未定
            }

            _commandUI.SetActive(false);
        }
    }

    public void StartPlayerTurn() 
    {
        _skillText[0].text = _playerStatus.NinjaThrowingKnives.SkillName;
        _skillText[1].text = _playerStatus.PlayerSkillList[0].SkillName;
        _skillText[2].text = _playerStatus.PlayerSkillList[1].SkillName;

        _commandUI.SetActive(true);
    }
}

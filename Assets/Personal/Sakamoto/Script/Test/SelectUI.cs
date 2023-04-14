using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
    [Tooltip("çsìÆÇåàÇﬂÇÈUI")]
    [SerializeField] private Transform[] _actionUi = new Transform[4];
    [SerializeField] private Transform[] _actionUiPos = new Transform[4];
    [SerializeField] private int _lotateNum;
    [SerializeField] private Image _skillPanel = default;

    private WeaponStatus _weaponStatus;

    private void Start()
    {
        _skillPanel.gameObject.SetActive(false);
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
                //éûåvâÒÇËÇÃèàóù
                nextMoveNum = ((i + 1) + _lotateNum) % _actionUi.Length;
            }
            else
            {
                //îºéûåvâÒÇË
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
                Debug.Log("skill view");
                _skillPanel.gameObject.SetActive(true);
            }
            else if (_actionUi[num] == _actionUi[2])
            {
                Debug.Log("none 1");
                //ñ¢íË
            }
            else
            {
                Debug.Log("none 2");
                //ñ¢íË
            }
        }
    }
}

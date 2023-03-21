using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectUI : MonoBehaviour
{
    [Tooltip("çsìÆÇåàÇﬂÇÈUI")]
    [SerializeField] private Transform[] _actionUi = new Transform[4];
    [SerializeField] private Transform[] _actionUiPos = new Transform[4];
    [SerializeField] private int _lotateNum;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
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
                     _actionUiPos[nextMoveNum].transform.localPosition, 1f);

            DOTween.To(() => _actionUi[i].transform.localScale,
                       x => _actionUi[i].transform.localScale = x,
                       _actionUiPos[nextMoveNum].transform.localScale, 1f);
        }
    }
}

using UnityEngine;
using DG.Tweening;

public class BattleSelect : MonoBehaviour
{
    [SerializeField] private Transform[] _battles = new Transform[4];

    private int _index = 0;

    public void MovePos(int num)
    {
        int n = Mathf.Abs(_index - num);

        for (int i = 0; i < _battles.Length; i++)
        {
            if (i != num)
            {
                if (_index < num)
                {
                    _battles[i].transform.DOLocalMoveX(
                        _battles[i].transform.localPosition.x + (-430f * n), 1f);
                }
                else if (_index > num)
                {
                    _battles[i].transform.DOLocalMoveX(
                        _battles[i].transform.localPosition.x + (430f * n), 1f);
                }
            }
            else
            {
                _battles[i].transform.DOLocalMoveX(-500f, 1f);
            }
        }
        _index = num;
    }
}

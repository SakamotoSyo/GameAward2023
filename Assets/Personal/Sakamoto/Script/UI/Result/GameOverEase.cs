using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

public class GameOverEase : MonoBehaviour
{
    [SerializeField] private float _moveY;
    [SerializeField] private Image[] _backGroundImage = new Image[2];
    [SerializeField] GameObject[] _gameOverImage = new GameObject[8];
    // Start is called before the first frame update
    async void Start()
    {
        await BackGroundFade();
        await GameOver();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private async UniTask BackGroundFade()
    {
        _backGroundImage[0].DOFade(endValue: 1f, duration: 1f);
        await _backGroundImage[1].DOFade(endValue: 1f, duration: 1f);
    }

    private async UniTask GameOver()
    {
        for (int i = 0; i < _gameOverImage.Length; i++)
        {
            _gameOverImage[i].transform.DOMove(new Vector3(_gameOverImage[i].transform.position.x, _moveY, 0), 1f)
                             .SetEase(Ease.OutBounce);
            await UniTask.Delay(100);
        }
    }
}

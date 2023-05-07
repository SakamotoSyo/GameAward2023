using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> バトルに挑まれている </summary>
public class Challenged : MonoBehaviour
{
    [SerializeField] private EnemyDataBase _enemyDataBase;
    [SerializeField] private GameObject _buttonInsPos;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _exclamationMarkObj;
    [SerializeField] private Button _challengedButton;
    [SerializeField] private int _rankPointRange = 100;
    [SceneName]
    [SerializeField] private string _battleScene;

    private void Start()
    {
        ChacckChallenged();
    }

    public void ChacckChallenged()
    {
        EnemyData[] enemyDataLow = _enemyDataBase.GetRankEnemyArrayData(GameManager.PlayerSaveData.PlayerRankPoint, _rankPointRange, false);
        if (enemyDataLow.Length == 0)
        {
            _challengedButton.interactable = false;
            _exclamationMarkObj.SetActive(false);
        }
        else 
        {
            _challengedButton.interactable = true;
            _exclamationMarkObj.SetActive(true);
        }
    }

    /// <summary> 「挑まれている」ボタンクリック時 </summary>
    public void PlayChallenged()
    {
        //自分より下位のランクの敵をランダムに選び、バトル開始
        EnemyData[] enemyDataLow = _enemyDataBase.GetRankEnemyArrayData(GameManager.PlayerSaveData.PlayerRankPoint, _rankPointRange, false);
        var randomIndex = Random.Range(0, enemyDataLow.Length);
        GameManager.SetEnemyData(enemyDataLow[randomIndex]);
        SceneLoader.LoadScene(_battleScene);
    }
}

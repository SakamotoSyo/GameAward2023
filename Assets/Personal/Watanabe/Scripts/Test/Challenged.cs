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
    [SerializeField] private int _rankPointRange = 100;
    [SceneName]
    [SerializeField] private string _battleScene;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    /// <summary> 「挑まれている」ボタンクリック時 </summary>
    public void PlayChallenged()
    {
        //自分より下位のランクの敵をランダムに選び、バトル開始

        EnemyData[] enemyDataLow = _enemyDataBase.GetEnemyArrayData(GameManager.PlayerSaveData.PlayerRankPoint, _rankPointRange, false);

        var enemyButtonPrefab = Instantiate(_enemyPrefab);
        enemyButtonPrefab.transform.SetParent(_buttonInsPos.transform);
        var randomIndex = Random.Range(0, enemyDataLow.Length);
        enemyButtonPrefab.GetComponent<Image>().sprite = enemyDataLow[randomIndex].EnemySprite;
        var button = enemyButtonPrefab.GetComponent<Button>();

        button.onClick.AddListener(() => GameManager.SetEnemyData(enemyDataLow[randomIndex]));
        button.onClick.AddListener(() => SceneLoader.LoadScene(_battleScene));
    }
}

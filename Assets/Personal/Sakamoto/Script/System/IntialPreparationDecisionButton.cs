using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntialPreparationDecisionButton : MonoBehaviour
{
    [SerializeField] private IntialPreparationScript _preparationScript;
    [SceneName]
    [SerializeField] private string _nextScene;

    public void NextSceneEvent()
    {
        if (_preparationScript.WeaponDatas.Count == 2)
        {
            var sequence = DOTween.Sequence();

            sequence.AppendCallback(() =>
                    {
                        SoundManager.Instance.CriAtomPlay(CueSheet.SE, "SE_Enter");
                    })
                    .AppendInterval(0.5f)
                    .AppendCallback(() =>
                    {
                        var playerData = new PlayerSaveData();
                        playerData.WeaponArray = _preparationScript.WeaponDatas.ToArray();
                        GameManager.SetPlayerData(playerData);
                        //SceneLoader.LoadScene(_nextScene);

                        SceneLoader.LoadScene(_nextScene);
                    });
        }
        else 
        {
            Debug.LogWarning("•Ší‚ğ‚Q–{‘I‚ñ‚Å‚­‚¾‚³‚¢");
        }
    }
}

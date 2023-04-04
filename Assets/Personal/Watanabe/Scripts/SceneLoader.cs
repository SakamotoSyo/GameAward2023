using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private readonly Dictionary<SceneName, string> _scenes = new()
    {
        [SceneName.TitleScene] = "Title",
        [SceneName.HomeScene] = "Home",
        [SceneName.RankScene] = "RankScene",
        [SceneName.ResultScene] = "Result",
    };

    public void Load(int num)
    {
        var sceneName = (SceneName)num;

        SceneManager.LoadScene(_scenes[sceneName]);
    }

    public void Load(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

public enum SceneName
{
    //ランクシーン名は仮
    TitleScene,
    HomeScene,
    RankScene,
    ResultScene,
}

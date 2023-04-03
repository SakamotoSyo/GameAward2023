using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private readonly Dictionary<SceneName, string> _scenes = new()
    {
        [SceneName.TitleScene] = "Title",
        [SceneName.GameScene] = "InGame",
        [SceneName.ResultScene] = "Result",
    };

    public void Load(int num)
    {
        var sceneName = (SceneName)num;

        SceneManager.LoadScene(_scenes[sceneName]);
    }
}

public enum SceneName
{
    TitleScene,
    GameScene,
    ResultScene,
}

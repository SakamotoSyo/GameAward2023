using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadScene(Action action, string name)
    {
        action?.Invoke();
        SceneManager.LoadScene(name);
    }

    public static void LoadScene(string sceneName)
    {
        // 

        SceneManager.LoadScene(sceneName);
    }
}

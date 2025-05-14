using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrapper : MonoBehaviour
{

}
public static class PerformBootstrap
{
    const string BootStrapSceneName = "Bootstrap";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void BootStrapGame()
    {
        //Traverse the currently loaded scenes
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
           Scene scene = SceneManager.GetSceneAt(i);

            if (scene.name == BootStrapSceneName)
            {
                return;
            }
        }
        SceneManager.LoadScene(BootStrapSceneName , LoadSceneMode.Additive);
    }
}
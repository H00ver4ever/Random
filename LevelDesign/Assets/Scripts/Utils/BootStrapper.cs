using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrapper : MonoBehaviour
{
    public static BootStrapper Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(this);

    }

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
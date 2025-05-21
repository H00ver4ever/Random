using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Optional: Make this a singleton if you want a single instance across scenes
    public static GameManager Instance;

    void Awake()
    {
        // Singleton setup (optional)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Exit game when Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }
    public void ExitGame()
    {
        Debug.Log("Exiting game...");
#if UNITY_EDITOR
        // Stop play mode in the editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit application in build
        Application.Quit();
#endif
    }

    
}


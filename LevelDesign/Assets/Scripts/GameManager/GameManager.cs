using System;
using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour
{
    public static LoadSavemanager StateManager
    {
        
        get
        {
            if (!stateManager)
                stateManager = instance.GetComponent<LoadSavemanager>();

            return stateManager;
        }
    }
    public static GameManager instance1
    {
        get
        {
            if (!instance1)
                instance = new GameObject("GameManager").AddComponent<GameManager>();
            return instance1;
        }
    }

    // Optional: Make this a singleton if you want a single instance across scenes
    public static GameManager instance;
    private static LoadSavemanager stateManager = null;

    // Called before Start on object creation
    void Awake ()
	{
		//Check if there is an existing instance of this object
		if((instance) && (instance.GetInstanceID() != GetInstanceID()))
			Destroy(gameObject); //Delete duplicate
		else
		{
			instance = this; //Make this object the only instance
			DontDestroyOnLoad (gameObject); //Set as do not destroy
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



    // Save Game
    public void SaveGame()
    {
        Debug.Log("testsavegameinGM");
        // Print the path where the XML is save
        Debug.Log(Application.persistentDataPath);

        // Call save game functionality
        StateManager.Save(Application.persistentDataPath + "/SaveGame.xml");
    }

    // Load Game
    public void LoadGame()
    {
        Debug.Log("TestLoadgameinGM");
        //Call load game functionality
        StateManager.Load(Application.persistentDataPath + "/SaveGame.xml");

        // Restart Level
        //RestartGame();
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


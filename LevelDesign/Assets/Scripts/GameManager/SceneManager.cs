using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

[Obsolete]
public class CanvasManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startBtn;
    public Button quitBtn;
    [Header("Menus")]
    public GameObject mainMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (startBtn) startBtn.onClick.AddListener(() => SceneManager.LoadScene("Game")); 
        if (quitBtn) quitBtn.onClick.AddListener(QuitGame);

    }

    private void OnDisable()
    {
        if (startBtn) startBtn.onClick.RemoveAllListeners();
        if (quitBtn) quitBtn.onClick.RemoveAllListeners();
        //if (mainMenuBtn) mainMenuBtn.onClick.RemoveAllListeners();
       
    }

    private void SetMenus(GameObject menuToActivate, GameObject menuToDeactivate)
    {
        if (menuToActivate) menuToActivate.SetActive(true);
        if (menuToDeactivate) menuToDeactivate.SetActive(false);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    // Update is called once per frame
    void Update()
    {
        
        

    }
}

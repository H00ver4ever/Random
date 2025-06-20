using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.TextCore.Text;

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


    public GameObject pausePanel;
    bool gamePaused;

    Player cRef;
    Enemy eRef;

    // Use this for initialization
    void Awake()
    {
        cRef = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        eRef = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
            PauseGame();
        
        Debug.Log("GamePaused");

    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        pausePanel.SetActive(gamePaused);

        if (gamePaused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void SaveGame()
    {
        Debug.Log("SavegamefunctioninSM");
        cRef.SaveGamePrepare();
        eRef.SaveGamePrepare();

        GameManager.instance.SaveGame();
    }

    public void LoadGame()
    {
        Debug.Log("LoadgamefunctioninSM");
        GameManager.instance.LoadGame();

        cRef.LoadGameComplete();
        eRef.LoadGameComplete();


    }
}

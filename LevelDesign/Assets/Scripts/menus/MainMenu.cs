using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu
{
    public Button startBtn;
    public Button quitBtn;
    
    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.MainMenu;

        if (startBtn) startBtn.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        if (quitBtn) quitBtn.onClick.AddListener(() => Application.Quit());
        
    }
}

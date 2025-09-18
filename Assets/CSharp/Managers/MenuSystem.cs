using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] private List<MenuScreen> menuScreens = new();
    [SerializeField] private string pauseMenuName = "Pause";
    private bool isPaused = false;

    public MenuScreen CurrentScreen { get; private set; }

    private MenuScreen FindByName(string name)
        => menuScreens.Find(ms => ms != null && ms.Name == name);

    public void ToggleMenuScreen(string name)
    {
        var target = FindByName(name);
        ToggleMenuScreen(target);
    }

    public void ToggleMenuScreen(MenuScreen screen)
    {
        if (screen == null) return;

        if (CurrentScreen != null && CurrentScreen != screen)
            CurrentScreen.Hide();

        screen.Toggle();
        CurrentScreen = (screen.ScreenObject != null && screen.ScreenObject.activeSelf) ? screen : null;
    }

    public void ToggleMainMenu() => ToggleMenuScreen("Main");
    public void ToggleSettingsMenu() => ToggleMenuScreen("Settings");
    public void TogglePauseMenu()
    {
        ToggleMenuScreen(pauseMenuName);

        if (!isPaused)
        {
            Time.timeScale = 0f;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1f;
            isPaused = false;
        }
    }

    private void Awake()
    {
        CurrentScreen = null;
        foreach (var ms in menuScreens)
        {
            if (ms?.ScreenObject == null) continue;
            if (ms.ScreenObject.activeSelf)
            {
                if (CurrentScreen == null) CurrentScreen = ms;
                else ms.Hide();
            }
        }
    }
}

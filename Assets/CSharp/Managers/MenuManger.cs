using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _loadMenu;
    [SerializeField] private GameObject _settingsMenu;


    private bool isPaused;

    private void Start()
    {
        _pauseMenu?.SetActive(false);
        _loadMenu?.SetActive(false);
        _settingsMenu.SetActive(false);

    }


    #region Pause/Unpause Functions

    public void TogglePauseMenu(CallbackContext ctx)
    {
        if (isPaused)
        {
            Unpause();
        }
        else
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        OpenPauseMenu();
    }   

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        CloseAllMenus();
    }

    #endregion


    #region Canvas Activations/Deactivations

    private void OpenPauseMenu()
    {
        _pauseMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _loadMenu.SetActive(false);
    }

    private void OpenSaveMenuHandle()
    {
        // Implement save menu logic here
        Debug.Log("Save Menu Opened - Functionality to be implemented");
    }

    private void OpenLoadMenuHandle()
    {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _loadMenu.SetActive(true);
    }

    private void OpenSettingMenuHandle()
    {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _loadMenu.SetActive(false);
    }

    
    private void CloseAllMenus()
    {
        _pauseMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _loadMenu.SetActive(false);
    }

    #endregion


    #region Pause Menu Button Actions

    public void OnResumePress()
    {
        Unpause();
    }

    public void OnSavePress()
    {
        OpenSaveMenuHandle();
    }

    public void OnLoadPress()
    {
        OpenLoadMenuHandle();
    }
    
    public void OnSettingsPress()
    {
        OpenSettingMenuHandle();
    }

    public void OnTitleScreenPress()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
    }

    public void OnQuitPress()
    {
        Application.Quit();
    }


    #endregion S

    #region Save Menu Button Actions    

    #endregion

    #region Load Menu Button Actions    

    public void OnLoadBackPress()
    {
        OpenPauseMenu();
    }

    #endregion

    #region Settings Menu Button Actions    

    public void OnSettingsBackPress()
    {
        OpenPauseMenu();
    }   

    #endregion


}

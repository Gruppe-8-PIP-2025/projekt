using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// WorldManager is the sole persistent object that follows the player between
/// scenes and contains and distributes data required to run the game.
/// </summary>
public class WorldManager : MonoBehaviour
{
    #region Constants
    private const float TRANSITION_TIME_MINIMUM = 2.0f;
    private const float SCENETRANSITIONTESTDELAY = 2.0f;
    private const string SCENETRANSTIONTESTORIGIN = "SceneTransitionTestOrigin";
  private const string SCENETRANSTIONTESTTARGET = "SceneTransitionTestTarget";
  #endregion

  #region Component Configuration
  [Header("Scene Transitions")]
  [SerializeField] private GameObject LoadingScreen;
  #endregion


  private SessionStatistics sessionStatistics;
  private GameFlagManager gameFlagManager;
  private SceneManager sceneManager;

  /// <summary>
  /// Initiates the process of moving the user to a different scene, utilizing a
  /// loading screen where applicable. This will disinherit the current
  /// SceneManager and attempt to find and adopt the local SceneManager upon
  /// arrival.
  /// </summary>
  public void SceneTransition(string gameScene)
  {
    StartCoroutine(nameof(LoadScene), gameScene);
  }

  /// <summary>
  /// Loads the current user's settings if they can be found. Otherwise this
  /// method will load the default settings and save them as the settings for
  /// this user.
  /// </summary>
  public void LoadUserSettings()
  {
    Debug.LogException(new NotImplementedException("LoadUserSettings is not yet implemented."));
  }

  /// <summary>
  /// Pauses the game by setting the tick-rate/rate of advancement within the
  /// scene to zero.
  /// </summary>
  /// <remarks>
  /// This method will accept various parameters, such as the muffling of
  /// ambient game audio or music. This method will purely pause the game in
  /// this way and calling it will not itself open a pause menu.
  /// </remarks>
  public void PauseGame()
  {
    Debug.LogException(new NotImplementedException("PauseGame is not yet implemented."));
  }

  /// <summary>
  /// Attempts to find and adopt the current SceneManager.
  /// </summary>
  public void AdoptSceneManager()
  {
    Debug.LogException(new NotImplementedException("AdoptSceneManager is not yet implemented."));
  }

  #region Coroutines
  private IEnumerator LoadScene(string value)
  {
    float timer = 0.0f;

    // Delay to avoid timing conflicts when testing scene-change on startup.
    if (value == SCENETRANSTIONTESTTARGET)
    {
      while (timer < SCENETRANSITIONTESTDELAY)
      {
        timer += Time.deltaTime;
        yield return null;
      }
    }
  
    LoadingScreen.SetActive(true);
    LoadingScreenWidget loadingScreenWidget = LoadingScreen.GetComponent<LoadingScreenWidget>();

    AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(value);
    asyncLoadScene.allowSceneActivation = false;
    timer = 0.0f;

    while (asyncLoadScene.progress < 0.9f || timer < TRANSITION_TIME_MINIMUM)
    {
      loadingScreenWidget.UpdateProgress(asyncLoadScene.progress);
      timer += Time.deltaTime;
      yield return null;
    }

    asyncLoadScene.allowSceneActivation = true;
    LoadingScreen.SetActive(false);
  }
  #endregion

  //SceneTransitionTest
  #region MonoBehavior
  /// <summary>
  /// Start is called once before the first execution of Update after the
  /// MonoBehaviour is created
  /// </summary>
  void Start()
  {
    DontDestroyOnLoad(gameObject);
    sessionStatistics = new();

    //SceneTransitonTest();
  }
  #endregion


  #region Test/Debug
  private void SceneTransitonTest()
  {
    SceneTransition(SCENETRANSTIONTESTTARGET);
  }
    #endregion

}

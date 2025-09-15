using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <author>
/// Maria Wickes (maria.lindling@protonmail.com)
/// </author>
/// <summary>
/// WorldManager is the main persistent object that follows the player between
/// scenes and contains and distributes data required to run the game.
/// </summary>
public class WorldManager : MonoBehaviour
{
  #region Constants
  /// <summary>Minimum time the loading screen remains active.</summary>
  private const float TRANSITION_TIME_MINIMUM = 0.66f;

  /// <summary>Origin Scene for SceneTransitionTest.</summary>
  private const string SCENETRANSTIONTESTORIGIN = "SceneTransitionTestOrigin";

  /// <summary>Destination/Target Scene for SceneTransitionTest.</summary>
  private const string SCENETRANSTIONTESTTARGET = "SceneTransitionTestTarget";
  #endregion

  #region Component Configuration
  [Header("Scene Transitions")]
  /// <summary>Field containing the GameObject that represents the LoadingScreen.</summary>
  [SerializeField] private GameObject loadingScreen;
  #endregion


  /// <summary>Object that handles statistics tracking for the current play-session.</summary>
  /// <remarks>Not implemented.</remarks>
  private SessionStatistics sessionStatistics;
  
  /// <summary>Object for tracking, setting and managing GameFlags.</summary>
  /// <remarks>Not implemented.</remarks>
  private GameFlagManager gameFlagManager;

  /// <summary>
  /// Initiates the process of moving the user to a different scene, utilizing a
  /// loading screen where applicable. This will disinherit the current
  /// SceneManager and attempt to find and adopt the local SceneManager upon
  /// arrival.
  /// </summary>
  /// <param name="sceneName">File(?)name of the scene to transition to.</param>
  /// <remarks>
  /// This method starts an asyncronous coroutine. The game will continue to run
  /// while the coroutine executed.
  /// </remarks>
  public void SceneTransition(string sceneName)
  {
    StartCoroutine(nameof(LoadScene), sceneName);
  }

  /// <summary>
  /// Loads the current user's settings if they can be found. Otherwise this
  /// method will load the default settings and save them as the settings for
  /// this user.
  /// </summary>
  /// <remarks>Not implemented.</remarks>
  public void LoadUserSettings()
  {
    Debug.LogException(new NotImplementedException("LoadUserSettings is not yet implemented."));
  }

  /// <summary>
  /// Pauses the game by setting the tick-rate/rate of advancement within the
  /// scene to zero.
  /// </summary>
  /// <remarks>
  /// Not implemented.
  /// This method will accept various parameters, such as the muffling of
  /// ambient game audio or music. This method will purely pause the game in
  /// this way and calling it will not itself open a pause menu.
  /// </remarks>
  public void PauseGame()
  {
    Debug.LogException(new NotImplementedException("PauseGame is not yet implemented."));
  }

  #region Coroutines
  /// <summary>
  /// Activates the loading screen and updates its state based on the progress of
  /// the load scene process, before de-activating the load screen when both
  /// minimum time has elapsed and the progress has reached completion.
  /// </summary>
  /// <param name="value">file(?)name of the destination scene</param>
  /// <returns>yields null on each iteration</returns>
  private IEnumerator LoadScene(string value)
  {
    float timer = 0.0f;

    // Delay to avoid timing conflicts when testing scene-change on startup.
    if (value == SCENETRANSTIONTESTTARGET)
    {
      while (timer < 1.0f)
      {
        timer += Time.deltaTime;
        yield return null;
      }
    }

    loadingScreen.SetActive(true);
    LoadingScreenWidget loadingScreenWidget = loadingScreen.GetComponent<LoadingScreenWidget>();

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
    loadingScreen.SetActive(false);
  }
  #endregion


  #region MonoBehavior
  /// <summary>
  /// Start is called once before the first execution of Update after the
  /// MonoBehaviour is created.
  /// </summary>
  void Start()
  {
    DontDestroyOnLoad(gameObject);
    sessionStatistics = new();

    // SceneTransitonTest();
  }
  #endregion


  #region Test/Debug
  /// <summary>
  /// Attempts to invoke SceneTransition to load the designated test scene.
  /// </summary>
  private void SceneTransitonTest()
  {
    SceneTransition(SCENETRANSTIONTESTTARGET);
  }
  #endregion
}

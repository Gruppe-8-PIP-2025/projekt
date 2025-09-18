using System;
using UnityEngine;

/// <author>
/// Maria Wickes (maria.lindling@protonmail.com)
/// </author>
/// <summary>
/// WorldManager is the main persistent object that follows the player between
/// scenes and contains and distributes data required to run the game.
/// </summary>
public partial class WorldManager : MonoBehaviour
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
  [Header("Menus and Screens")]
  /// <summary>Field containing the GameObject that represents the LoadingScreen.</summary>
  [SerializeField] private GameObject loadingScreen;

  /// <summary>Field containing the MenuSystem to handle, display and navigate between menu screens.</summary>
  [SerializeField] private MonoBehaviour menuSystem;
  #endregion


  #region Properties
  /// <summary>Field containing the GridManager meant to control and interpret the play area.</summary>
  public MonoBehaviour GridManager { get; private set; }

  /// <summary>Field containing the CameraController to control and limit player camera movement.</summary>
  public MonoBehaviour CameraController { get; private set; }
  #endregion


  #region Scene Control
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
  #endregion


  #region Settings
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
  #endregion


  #region MenuSystem
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
  #endregion


  #region MonoBehavior
  /// <summary>
  /// Awake is called once before the first execution of Update and Start after
  /// the MonoBehaviour is created. It will be called even if this GameObject is
  /// not active. 
  /// </summary>
  void Awake()
  {
    DontDestroyOnLoad(gameObject);
    sessionStatistics = new();
  }

  /// <summary>
  /// Start is called once before the first execution of Update after the
  /// MonoBehaviour is created.
  /// </summary>
  void Start()
  {
  }

  /// <summary>
  /// Update is called once every frame.
  /// </summary>
  void Update()
  {
  }
  #endregion


  #region X
  // Implementation Suspended until Critical Issues are resolved.

  /// <summary>Object that handles statistics tracking for the current play-session.</summary>
  /// <remarks>Not implemented.</remarks>
  private SessionStatistics sessionStatistics;

  /// <summary>Object for tracking, setting and managing GameFlags.</summary>
  /// <remarks>Not implemented.</remarks>
  private GameFlagManager gameFlagManager;
  #endregion
}

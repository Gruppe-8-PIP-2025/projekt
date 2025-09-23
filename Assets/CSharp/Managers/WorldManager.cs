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
  #region Instance Control
  public static WorldManager Instance { get; private set; }
  #endregion

  #region Constants
  /// <summary>Minimum time the loading screen remains active.</summary>
  private const float TRANSITION_TIME_MINIMUM = 1.6667f;

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
  [SerializeField] private MenuManager menuManager;
  #endregion


  #region Properties
  /// <summary>Field containing the GridManager meant to control and interpret the play area.</summary>
  public MonoBehaviour GridManager { get; private set; }

  /// <summary>Field containing the CameraController to control and limit player camera movement.</summary>
  public MonoBehaviour CameraController { get; private set; }
  #endregion


  #region Scene Control
  public void QuitGame()
  {
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #else
      Application.Quit();
    #endif
  }

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
    AdoptGridManager();
  }

  /// <summary>
  /// Attempts to find and adopt the local GridManager for the current scene. 
  /// </summary>
  /// <typeparam name="GridManager"></typeparam>
  private void AdoptGridManager() =>
    GridManager = GameObject.Find("GridManager").GetComponent<GridManager>();
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


  public void ResetControlVariables()
  {
    Debug.LogException(new NotImplementedException("ResetControlVariables is not yet implemented."));
  }

  #region MonoBehavior
  /// <summary>
  /// Awake is called once before the first execution of Update and Start after
  /// the MonoBehaviour is created. It will be called even if this GameObject is
  /// not active. 
  /// </summary>
  void Awake()
  {
    if (Instance != null)
    {
      Debug.LogWarning("Duplicate WorldManager detected.");
      Destroy(gameObject);
    }
    else
    {
      Instance = this;
    }

    DontDestroyOnLoad(gameObject);
    sessionStatistics = new();
  }

  /// <summary>
  /// Start is called once before the first execution of Update after the
  /// MonoBehaviour is created.
  /// </summary>
  void Start()
  {
    AdoptGridManager();
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

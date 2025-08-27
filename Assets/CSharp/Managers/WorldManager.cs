using UnityEngine;

/// <summary>
/// WorldManager is the sole persistent object that follows the player between
/// scenes and contains and distributes data required to run the game.
/// </summary>
public class WorldManager : MonoBehaviour
{
  private SessionStatistics sessionStatistics;
  private GameFlagManager gameFlagManager;
  private SceneManager sceneManager;

  /// <summary>
  /// Initiates the process of moving the user to a different scene, utilizing a
  /// loading screen where applicable. This will disinherit the current
  /// SceneManager and attempt to find and adopt the local SceneManager upon
  /// arrival.
  /// </summary>
  public void SceneTransition()
  {

  }

  /// <summary>
  /// Loads the current user's settings if they can be found. Otherwise this
  /// method will load the default settings and save them as the settings for
  /// this user.
  /// </summary>
  public void LoadUserSettings()
  {

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

  }

  /// <summary>
  /// Attempts to find and adopt the current SceneManager.
  /// </summary>
  public void AdoptSceneManager()
  {

  }



  /// <summary>
  /// Start is called once before the first execution of Update after the
  /// MonoBehaviour is created
  /// </summary>
  void Start()
  {
    DontDestroyOnLoad(gameObject);
    sessionStatistics = new ();
  }
}

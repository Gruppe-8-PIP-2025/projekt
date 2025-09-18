using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <author>
/// Maria Wickes (maria.lindling@protonmail.com)
/// </author>
/// <summary>
/// This class gives the WorldManager a convenient all-in-one object with which
/// it can cover a SceneTransition with a loading screen.
/// </summary>
/// <remarks>
/// This class should not be accessed anywhere outside of WorldManager.
/// </remarks>
public class LoadingScreenWidget : MonoBehaviour
{
  #region Constants
  /// <summary>Default value of the 'tips and tricks' text field.</summary>
  private const string DEFAULT_TIPSANDTRICKS = "Tipp: Du kannst mit der Methode \"SetTipsAndTricksText\" dieses Feld mit hilfreichen Hinweisen füllen!";

  /// <summary>Default value of the 'loading info' text field.</summary>
  private const string DEFAULT_LOADINGINFO = "Loading...";
  #endregion


  #region Component Configuration
  [Header("Graphical Assets")]
  /// <summary>Component containing the 'splash graphic' that is featured on the loading screen.</summary>
  /// <remarks>Tested and working with PNG(698 x 247 px)</remarks>
  [SerializeField] private RawImage splashGraphic;

  /// <summary>Component containing the logo that is featured on the loading screen.</summary>
  /// <remarks>Tested and working with PNG(781 x 826 px)</remarks>
  [SerializeField] private RawImage logoGraphic;

  [Header("Tipps and Tricks")]
  /// <summary>The text field where 'tips and tricks' are displayed. May be empty.</summary>
  [SerializeField] private TextMeshProUGUI tipsAndTricks;

  [Header("Progress Bar")]
  /// <summary>
  /// An ordered list of progress image Components that will be sequentially
  /// activated to indicate how much progress has been made loading the next
  /// scene.
  /// </summary>
  /// <remarks>Tested and working with PNG(284 x 114 px)</remarks>
  [SerializeField] private List<RawImage> progresIndicators;

  /// <summary>The text field where 'loading info' is displayed. May be empty.</summary>
  [SerializeField] private TextMeshProUGUI loadingInfo;
  #endregion


  #region Properties
  #endregion


  #region Public Methods
  /// <summary>
  /// Replaces the current splash graphic with the given Texture.
  /// </summary>
  /// <param name="value">Texture sutable for a RawImage Component</param>
  public void SetSplashGraphic(Texture value)
    => splashGraphic.texture = value;

  /// <summary>
  /// Replaces the current logo graphic with the given Texture.
  /// </summary>
  /// <param name="value">Texture sutable for a RawImage Component</param>
  public void SetLogoGraphic(Texture value)
    => logoGraphic.texture = value;

  /// <summary>
  /// Replaces the current 'tips and tricks' text with the given string.
  /// </summary>
  /// <param name="value">text suitable for displaying to the player</param>
  /// <remarks>
  /// Not tested with special characters.
  /// It's recommended but not required to for the text to begin with "Tipp:".
  /// </remarks>
  public void SetTipsAndTricksText(string value)
    => tipsAndTricks.SetText(value);

  /// <summary>
  /// Replaces the current 'loading info' text with the given string.
  /// </summary>
  /// <param name="value">text suitable for displaying to the player</param>
  public void SetLoadingInfoText(string value)
    => loadingInfo.SetText(value);

  /// <summary>
  /// Updates the ProgressBar UI-Element with the current progress of
  /// the scene being loaded.
  /// </summary>
  /// <param name="progress">a number between 0.0f and 1.0f that represents
  /// how much of the next scene has been loaded</param>
  /// <remarks>
  /// This method should be called by WorldManager during scene-transition.
  /// </remarks>
  public void UpdateProgress(float progress)
  {
    if (progress < 0.0f && progress > 1.0f)
    {
      throw new IndexOutOfRangeException(
        "Loading progress cannot be less than 0% or greater than 100%."
        + $"The value \"{progress}\" is invalid."
      );
    }

    int enabledIndicators = (int)Math.Round(progresIndicators.Count * progress);
    for (int i = 0; i < progresIndicators.Count; i++)
    {
      progresIndicators[i].enabled = i < enabledIndicators;
    }
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

    UpdateProgress(0.0f);
    SetLoadingInfoText(DEFAULT_LOADINGINFO);
    SetTipsAndTricksText(DEFAULT_TIPSANDTRICKS);

    gameObject.SetActive(false);
  }

  /// <summary>
  /// Start is called once before the first execution of Update after the
  /// MonoBehaviour is created.
  /// </summary>
  public void Start()
  {
  }
  #endregion


  #region Test/Debug
  private void TestBattery1()
  {
    TestTipsAndTricks();
    UpdateProgress(0.66f);
    TestLoadingInfo();
  }

  private void TestTipsAndTricks()
  {
    Debug.Log($"Current TipsAndTricks Text: \"{tipsAndTricks.text}\"");
    SetTipsAndTricksText("Tipp: By testing your code, you ensure that it works. Probably. Maybe. Sometimes. Good luck!");
    Debug.Log($"New TipsAndTricks Text: \"{tipsAndTricks.text}\"");
  }

  private void TestLoadingInfo()
  {
    Debug.Log($"Current LoadingInfo Text: \"{loadingInfo.text}\"");
    SetLoadingInfoText("Loading.!?");
    Debug.Log($"New LoadingInfo Text: \"{loadingInfo.text}\"");
  }
  #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenWidget : MonoBehaviour
{
  #region Constants
  private const string DEFAULT_TIPSANDTRICKS = "Tipp: Du kannst mit der Methode \"SetTipsAndTricksText\" dieses Feld mit hilfreichen Hinweisen füllen!";
  private const string DEFAULT_LOADINGINFO = "Loading...";
  #endregion


  #region Component Configuration
  [Header("Graphical Assets")]
  [SerializeField] private RawImage SplashGraphic;
  [SerializeField] private RawImage LogoGraphic;

  [Header("Tipps and Tricks")]
  [SerializeField] private TextMeshProUGUI TipsAndTricks;

  [Header("Progress Bar")]
  [SerializeField] private List<RawImage> ProgresIndicators;
  [SerializeField] private TextMeshProUGUI LoadingInfo;
  #endregion


  #region Properties
  #endregion


  #region Public Methods
  public void SetSplashGraphic(Texture value)
    => SplashGraphic.texture = value;

  public void SetLogoGraphic(Texture value)
    => LogoGraphic.texture = value;

  public void SetTipsAndTricksText(string value)
    => TipsAndTricks.SetText(value);

  /// <remarks>This method should be called by WorldManager during scene-
  /// transition.</remarks>
  /// <summary>Updates the ProgressBar UI-Element with the current progress of
  /// the scene being loaded.</summary>
  /// <param name="progress">a number between 0.0 and 1.0 that represents
  /// how much of the next scene has been loaded</param>
  public void UpdateProgress(float progress)
  {
    if (progress < 0.0f && progress > 1.0f)
    {
      throw new IndexOutOfRangeException(
        "Loading progress cannot be less than 0% or greater than 100%."
        + $"The value \"{progress}\" is invalid."
      );
    }

    int enabledIndicators = (int)Math.Round(ProgresIndicators.Count * progress);
    for (int i = 0; i < ProgresIndicators.Count; i++)
    {
      ProgresIndicators[i].enabled = i < enabledIndicators;
    }
  }

  public void SetLoadingInfoText(string value)
    => LoadingInfo.SetText(value);
  #endregion
  

  #region MonoBehavior
  public void Start()
  {
    UpdateProgress(0.0f);
    SetLoadingInfoText(DEFAULT_LOADINGINFO);
    SetTipsAndTricksText(DEFAULT_TIPSANDTRICKS);

    //TestBattery1();
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
    Debug.Log($"Current TipsAndTricks Text: \"{TipsAndTricks.text}\"");
    SetTipsAndTricksText("Tipp: By testing your code, you ensure that it works. Probably. Maybe. Sometimes. Good luck!");
    Debug.Log($"New TipsAndTricks Text: \"{TipsAndTricks.text}\"");
  }

  private void TestLoadingInfo()
  {
    Debug.Log($"Current LoadingInfo Text: \"{LoadingInfo.text}\"");
    SetLoadingInfoText("Loading.!?");
    Debug.Log($"New LoadingInfo Text: \"{LoadingInfo.text}\"");
  }
  #endregion
}

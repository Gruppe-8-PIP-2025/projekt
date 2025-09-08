using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class LoadingScreenProgressBar : MonoBehaviour
{
  [Header("Progress Indicators")]
  [SerializeField] private List<GameObject> ProgresIndicators;

  private List<RawImage> RawImages
  {
    get
    {
      return ProgresIndicators.Select(pi => pi.GetComponent<RawImage>()).ToList();
    }
  }

  /// <summary>
  /// 
  /// </summary>
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
    for (int i = 0; i < RawImages.Count; i++)
    {
      RawImages[i].enabled = i < enabledIndicators;
    }
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    
  }

  // Update is called once per frame
  void Update()
  {
    /* Test */
    UpdateProgress(0.8f);
    /* Test */
  }
}

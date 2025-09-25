using System;
using Unity;
using Unity.VisualScripting;
using UnityEngine;

// TODO: fade-in/out is inconsistent with opacity
/// <author>Maria Wickes (maria.lindling@protonmail.com)</author>
/// <summary>
/// This class tracks and manages the status of an active popup message.
/// </summary>
class ActivePopUp
{
  #region Fields
  /// <summary>The text of the popup message.</summary>
  private readonly string _text;
  
  /// <summary>The color of the popup message.</summary>
  private readonly Color _color;

  /// <summary>The initial duration of the popup message.</summary>
  private readonly float _initialDuration;

  /// <summary>The time at which the popup message first began to be displayed.</summary>
  private float _startTime;

  /// <summary>The time at which the popup message expires.</summary>
  private float _endTime;

  /// <summary>The point in time at which the text is fully faded in.</summary>
  private float _fadeInMark;

  /// <summary>The point in time at which the text begins fading out.</summary>
  private float _fadeOutMark;
  #endregion


  #region Public Properties
  /// <summary>The text of the popup message.</summary>
  public string Text => _text;

  /// <summary>The color of the popup message.</summary>
  public Color Color => _color.WithAlpha(Opacity);

  /// <summary>The time at which the popup message expires.</summary>
  public float EndTime => _endTime;

  /// <summary>Whether or not the popup message is expired.</summary>
  public bool IsExpired => Time.time >= _endTime;
  #endregion


  #region Private Properties
  /// <summary>The opacity of the popup message based on its fade-in/out progress.</summary>
  private float Opacity => IsFadingIn ? FadeInPercentage : (IsFadingOut ? 1.0f - FadeOutPercentage : 1.0f);

  /// <summary>Value ranging from 0.0f to 1.0f representing how far the popup message has faded in.</summary>
  private float FadeInPercentage => Mathf.Clamp((Time.time - _startTime) / (_fadeInMark - _startTime),0.0f,1.0f);

  /// <summary>Value ranging from 0.0f to 1.0f representing how far the popup message has faded out.</summary>
  private float FadeOutPercentage => Mathf.Clamp((Time.time - _fadeOutMark) / (_endTime - _fadeOutMark),0.0f,1.0f);

  /// <summary>Whether or not the popup message is fading in.</summary>
  private bool IsFadingIn => Time.time <= _fadeInMark;

  /// <summary>Whether or not the popup message is fading out.</summary>
  private bool IsFadingOut => Time.time >= _fadeOutMark;
  #endregion


  #region Control Methods
  /// <summary>
  /// Sets the time for how long the fade-in of the popup message is.
  /// </summary>
  /// <param name="duration">the fade-out duration</param>
  public void SetFadeIn(TimeSpan duration)
  {
    _fadeInMark = _startTime + (float)duration.TotalSeconds;
  }

  /// <summary>
  /// Sets the time for how long the fade-out of the popup message is.
  /// </summary>
  /// <param name="duration">the fade-out duration</param>
  public void SetFadeOut(TimeSpan duration)
  {
    _fadeOutMark = _endTime - (float)duration.TotalSeconds;
  }

  /// <summary>
  /// Extends the duration of the popup message by the given amount.
  /// </summary>
  /// <param name="duration">the added duration</param>
  public void ExtendDuration(TimeSpan duration)
  {
    _endTime += (float)duration.TotalSeconds;
    _fadeOutMark += (float)duration.TotalSeconds;
  }

  /// <summary>
  /// Initiates the time-sensitive components of ActivePopUP as the popup
  /// message handler.
  /// </summary>
  public void Fire()
  {
    _startTime = Time.time;
    _endTime = Time.time + _initialDuration;
  }
  #endregion


  #region Constructor
  /// <param name="text">The text of the popup message.</param>
  /// <param name="color">The color of the popup message.</param>
  /// <param name="duration">The duration of the popup message.</param>
  public ActivePopUp(string text, Color color, TimeSpan duration)
  {
    _text = text;
    _color = color;
    _initialDuration = (float)duration.TotalSeconds;
  }
  #endregion
}
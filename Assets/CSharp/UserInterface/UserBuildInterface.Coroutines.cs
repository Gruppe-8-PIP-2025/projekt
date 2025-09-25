using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public partial class UserBuildInterface
{

  /// <summary>
  /// Begins the display of the given popup message and attempts to then display
  /// the next message in the queue, if any remain.
  /// </summary>
  /// <param name="activePopUp">the message being fired</param>
  private IEnumerator FirePopUp(ActivePopUp activePopUp)
  {
    _activePopUp = activePopUp; //cradle

    _activePopUp.SetFadeIn(new TimeSpan(0, 0, 0, 0, (int)(1000 * fadeDuration)));
    _activePopUp.SetFadeOut(new TimeSpan(0, 0, 0, 0, (int)(1000 * fadeDuration)));

    _activePopUp.Fire();

    while (!_activePopUp.IsExpired)
    {
      popUpLabel.text = _activePopUp.Text;
      popUpLabel.color = _activePopUp.Color;
      yield return null;
    }

    _activePopUp = null; //grave

    if (ReadyToFirePopUp)
      FireNextPopUp();
  }
}

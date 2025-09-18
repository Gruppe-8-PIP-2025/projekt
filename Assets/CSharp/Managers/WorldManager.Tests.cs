
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

public partial class WorldManager : MonoBehaviour
{
  #region Test/Debug
  /// <summary>
  /// Attempts run the full battery of WorldManager tests.
  /// </summary>
  /// <param name="ctx">Unity InputSystem context</param>
  public void RunTestBattery(CallbackContext ctx)
  {
    SceneTransitonTest();
  }

  /// <summary>
  /// Attempts to invoke SceneTransition to load the designated test scene.
  /// </summary>
  private void SceneTransitonTest()
  {
    SceneTransition(SCENETRANSTIONTESTTARGET);
  }
  #endregion
}
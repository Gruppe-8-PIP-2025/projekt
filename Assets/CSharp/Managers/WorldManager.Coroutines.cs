
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class WorldManager : MonoBehaviour
{
  #region SceneTransition
  /// <summary>
  /// Activates the loading screen and updates its state based on the progress of
  /// the load scene process, before de-activating the load screen when both
  /// minimum time has elapsed and the progress has reached completion.
  /// </summary>
  /// <param name="value">file(?)name of the destination scene</param>
  /// <returns>yields null on each iteration</returns>
  private IEnumerator LoadScene(string value)
  {
    loadingScreen.SetActive(true);
    LoadingScreenWidget loadingScreenWidget = loadingScreen.GetComponent<LoadingScreenWidget>();

    AsyncOperation asyncLoadScene = SceneManager.LoadSceneAsync(value);
    asyncLoadScene.allowSceneActivation = false;
    float timer = 0.0f;

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
}
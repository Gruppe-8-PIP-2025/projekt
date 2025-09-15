using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class UserBuildInterface : MonoBehaviour
{

  [SerializeField] private GameObject buildablesPanel;

  [SerializeField] private List<BuildableScriptableObject> availableBuildables;


  public void ToggleBuildablesPanel()
  {
    buildablesPanel.SetActive(!buildablesPanel.activeSelf);
  }

  public void ToggleBuildablesPanel(CallbackContext ctx)
  {
    buildablesPanel.SetActive(!buildablesPanel.activeSelf);
  }


  #region MonoBehavior
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  #endregion
}

using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UserBuildInterface : MonoBehaviour
{

  [SerializeField] private GameObject builtableButtonPrefab;

  [SerializeField] private GameObject buildablesPanel;

  [SerializeField] private List<BuildableScriptableObject> availableBuildables;

  private List<GameObject> buildableButtons;


  public void ToggleBuildablesPanel()
  {
    buildablesPanel.SetActive(!buildablesPanel.activeSelf);
  }

  public void ToggleBuildablesPanel(CallbackContext ctx)
  {
    buildablesPanel.SetActive(!buildablesPanel.activeSelf);
  }

  private void CreateBuildableButton(BuildableScriptableObject value)
  {
    GameObject buildableButton = Instantiate(builtableButtonPrefab);
    TMP_Text buildableButtonLabel = buildableButton
      .GetComponentsInChildren<TMP_Text>()
        .FirstOrDefault();

    buildableButton.GetComponent<Button>().onClick.AddListener(delegate
    {
      Debug.Log($"Attempt to build {buildableButtonLabel.text}."); // TODO
    }); 
    buildableButton.GetComponent<RawImage>().texture = value.MenuIcon;
    buildableButtonLabel.text = value.LocalizedName;

    buildableButton.transform.SetParent(buildablesPanel.transform);
    buildableButtons.Add(buildableButton);
  }

  private void DestroyBuildableButton(GameObject value)
  {
    buildableButtons.Remove(value);
    Destroy(value);
  }

  private void InitializeBuildableButtons()
  {
    foreach (BuildableScriptableObject entry in availableBuildables)
    {
      CreateBuildableButton(entry);
    }
  }


  void Awake()
  {
    buildableButtons = new();
  }

  #region MonoBehavior
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    InitializeBuildableButtons();
  }

  // Update is called once per frame
  void Update()
  {

  }
  #endregion
}

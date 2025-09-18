using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

public class UserBuildInterface : MonoBehaviour
{
  #region Unity Editor Fields
  [Header("Gameplay")]
  /// <summary>The buildables available to the player.</summary>
  [SerializeField] private List<BuildableEntry> availableBuildables;

  [Header("Prefabs")]
  /// <summary>The prefab used to create the buildables buttons.</summary>
  [SerializeField] private GameObject builtableButtonPrefab;

  [Header("Interface Components")]
  /// <summary>The GameObject representing the buildables panel.</summary>
  [SerializeField] private GameObject buildablesPanel;

  /// <summary>The components of the interface relevant to CursorOnInterface.</summary>
  [SerializeField] private List<GameObject> buildInterfaceComponents;
  #endregion

  #region Private Fields
  /// <summary>A list of the buildable buttons as their top-level GameObjects.</summary>
  private List<GameObject> _buildableButtons;
  #endregion

  #region Public Properties
  /// <summary>
  /// Returns true if the mouse cursor is hovering over a component of this
  /// interface.
  /// </summary>
  public bool CursorOnInterface
  {
    get
    {
      return buildInterfaceComponents.Select(bic =>
        bic.activeSelf == true &&
        RectTransformUtility.RectangleContainsScreenPoint(
          bic.GetComponent<RectTransform>(),
          Mouse.current.position.ReadValue())
        ).Any();
    }
  }
  #endregion


  #region Control Methods
  /// <summary>Toggles the active state of the Buildables Panel.</summary>
  public void ToggleBuildablesPanel()
  {
    buildablesPanel.SetActive(!buildablesPanel.activeSelf);
  }

  /// <summary>Toggles the active state of the Buildables Panel.</summary>
  /// <param name="ctx">CallbackContext of the interaction</param> 
  public void ToggleBuildablesPanel(CallbackContext ctx) =>
    ToggleBuildablesPanel();
  #endregion


  #region ScriptableObject Handling
  // TODO: attach the method that initializes the build process to the button
  /// <summary>
  /// Creates a button with values derived from the BuildableScriptableObject's
  /// properties and attaches a function that initiates the build process to the
  /// button's onClick listeners.
  /// </summary>
  /// <param name="value">the scriptable object being used as a base</param>
  private void CreateBuildableButton(BuildableEntry value)
  {
    GameObject buildableButton = Instantiate(builtableButtonPrefab);
    TMP_Text buildableButtonLabel = buildableButton
      .GetComponentsInChildren<TMP_Text>()
        .FirstOrDefault();

    buildableButton.GetComponent<Button>().onClick.AddListener(delegate
    {
      Debug.Log($"Attempt to build {buildableButtonLabel.text}."); // TODO
    });
    buildableButton.GetComponent<RawImage>().texture = value.Buildable.MenuIcon;
    buildableButtonLabel.text = value.Buildable.LocalizedName;

    buildableButton.transform.SetParent(buildablesPanel.transform);
    _buildableButtons.Add(buildableButton);
  }

  /// <summary>Destroys the given object.</summary>
  private void DestroyBuildableButton(GameObject value)
  {
    _buildableButtons.Remove(value);
    Destroy(value);
  }

  private void SetBuildableButtonEnabled(GameObject value)
  {

  }

  private void SetBuildableButtonDisabled(GameObject value)
  {

  }

  /// <summary>
  /// Populates the buildables panel with buttons derived from the available
  /// buildables.
  /// </summary>
  private void InitializeBuildableButtons()
  {
    foreach (BuildableEntry entry in availableBuildables)
    {
      CreateBuildableButton(entry);
    }
  }
  #endregion


  #region MonoBehavior
  /// <summary>
  /// Awake is called once after the MonoBehaviour is created, before Start is
  /// called and will be called even if the MonoBehavior is disabled.
  /// </summary>
  void Awake()
  {
    _buildableButtons = new();
    buildablesPanel.SetActive(false);
  }

  /// <summary>
  /// Start is called once before the first execution of Update after the
  /// MonoBehaviour is created.
  /// </summary>
  void Start()
  {
    InitializeBuildableButtons();
  }

  /// <summary>
  /// Update is called once per frame.
  /// </summary>
  void Update()
  {

  }
  #endregion
}

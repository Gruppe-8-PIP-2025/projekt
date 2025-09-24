using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

/// <author>
/// Maria Wickes (maria.lindling@protonmail.com)
/// </author>
/// <summary>
/// Object providing control and configuration of the user interface used for
/// gameplay aspects such as building.
/// </summary>
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
    Debug.Log("Ping");
    buildablesPanel.SetActive(!buildablesPanel.activeSelf);
  }

  /// <summary>Toggles the active state of the Buildables Panel.</summary>
  /// <param name="ctx">CallbackContext of the interaction</param> 
  public void ToggleBuildablesPanel(CallbackContext ctx) =>
    ToggleBuildablesPanel();
  #endregion


  #region ScriptableObject Handling
  /// <summary>
  /// Creates a button with values derived from the BuildableScriptableObject's
  /// properties and attaches a function that initiates the build process to the
  /// button's onClick listeners.
  /// </summary>
  /// <param name="value">the scriptable object being used as a base</param>
  private void CreateBuildableButton(BuildableEntry value)
  {
    value.Button = Instantiate(builtableButtonPrefab);
    value.Label = value.Button
      .GetComponentsInChildren<TMP_Text>()
        .FirstOrDefault();

    value.Button.GetComponent<RawImage>().texture = value.Buildable.MenuIcon;
    value.Label.text = value.Buildable.LocalizedName;

    if (value.Enabled)
    {
      SetBuildableButtonEnabled(value);
    }
    else
    {
      SetBuildableButtonDisabled(value);
    }

    value.Button.transform.SetParent(buildablesPanel.transform);
    _buildableButtons.Add(value.Button);
  }

  /// <summary>Destroys the given object.</summary>
  /// <param name="value">The GameObject to be destroyed.</param>
  private void DestroyBuildableButton(GameObject value)
  {
    _buildableButtons.Remove(value);
    Destroy(value);
  }

  /// <summary>
  /// Destroys the button component of the BuildableEntry and clears the
  /// affected properties.
  /// </summary>
  /// <param name="value">
  /// The BuildableEntry hosting the button to be destroyed.
  /// </param>
  private void DestroyBuildableButton(BuildableEntry value)
  {
    DestroyBuildableButton(value.Button);
    value.Button = null;
    value.Label = null;
  }

  // TODO: attach the method that initializes the build process to the button
  /// <summary>Switches the BuildableButton to the Enabled configuration.</summary>
  private void SetBuildableButtonEnabled(BuildableEntry value)
  {
    value.Button.GetComponent<Button>().onClick.AddListener(delegate
    {
      Debug.Log($"Attempt to build {value.Label.text}."); // TODO
    });
    // TODO: visual formatting of "enabled" state
    value.Label.color = Color.green;
  }

  /// <summary>Switches the BuildableButton to the Disabled configuration.</summary>
  private void SetBuildableButtonDisabled(BuildableEntry value)
  {
    value.Button.GetComponent<Button>().onClick.RemoveAllListeners();
    // TODO: visual formatting of "disabled" state
    value.Label.color = Color.red;
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

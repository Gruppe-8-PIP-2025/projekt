using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Microsoft.Win32.SafeHandles;
using static UnityEngine.InputSystem.InputAction;
using System;

/// <author>
/// [Can Özbal] ([canoezbal@gmail.com])
///</author>
/// <summary>
/// Controls camera movement, zoom, and constraints relative to a map in Unity.
/// </summary>
/// <remarks>
/// Attach this to a GameObject in Unity that serves as a camera controller. 
/// Requires a Cinemachine OrbitalFollow component for zoom functionality.
/// </remarks>
public class CameraController : MonoBehaviour
{
  #region Unity Editor Fields

  [Header("Movement")]
  [SerializeField] float moveSpeed = 250f;
  [SerializeField] float acceleration = 500f;
  [SerializeField] float aeceleration = 500f;
  [SerializeField] AnimationCurve moveSpeedZoomCurve = AnimationCurve.Linear(0f, 0.5f, 1f, 1f);
  [SerializeField] float mouseDragSpeed = 0.16667f;

  [Header("GridManager")]
  [SerializeField] GridManager gridManager; // Reference to your map object (e.g., Plane/Terrain).

  [Header("Zoom")]
  [SerializeField] float zoomSpeed = 60.0f;
  [SerializeField] float zoomSmoothing = 30.0f;
  [SerializeField] float zoomClosest = 0.0f;
  [SerializeField] float zoomFurthest = 15.0f;

  [Header("Components")]
  [SerializeField] Transform cameraTarget;
  [SerializeField] CinemachineOrbitalFollow orbitalFollow;

  #endregion

  #region Misc Variables
  private Vector2 moveInput;
  private Vector2 lookInput;
  private Vector2 scrollInput;
  private bool middleClickInput;
  private Vector2 edgeScrollInput;

  private float CurrentZoomSpeed = 0f;
  private Vector3 Velocity = Vector3.zero;
  private Bounds mapBounds;

  private bool isDragging = false;
  private Vector2 lastMousePosition;
  #endregion

  #region Properties
  /// <summary>
  /// Gets the current zoom level, normalized between 0 (zoomed in) and 1 (zoomed out).
  /// </summary>
  public float ZoomLevel
  {
      get
      {
          InputAxis axis = orbitalFollow.RadialAxis;
          return Mathf.InverseLerp(axis.Range.x, axis.Range.y, axis.Value);
      }
  }
  #endregion

  #region InputSystem EventHandlers
  /// <summary>
  /// Handles movement input.
  /// </summary>
  public void OnMove(CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();

  /// <summary>
  /// Handles look input (currently unused).
  /// </summary>
  public void OnLook(CallbackContext ctx) => lookInput = ctx.ReadValue<Vector2>();

  /// <summary>
  /// Handles scroll input for zoom.
  /// </summary>
  public void OnScroll(CallbackContext ctx) => scrollInput = ctx.ReadValue<Vector2>();

  /// <summary>
  /// Handles middle mouse button input (not currently used for dragging, handled in LateUpdates
  /// </summary>
  /// <remarks>Refactored this to work with the InputSystem we are using.</remarks>
  public void OnMiddleClick(CallbackContext ctx) => MouseDragLogic(ctx);

  private void MouseDragLogic(CallbackContext ctx)
  {
    if (ctx.action.name == "ClickM3")
    {
      // Start drag (LMB or MMB)
      if (!isDragging && ctx.ReadValueAsButton())
      {
        isDragging = true;
      }
      // End drag
      else if (isDragging && !ctx.ReadValueAsButton())
      {
        isDragging = false;
      }
    }
    // While dragging, move camera
    else if (isDragging && ctx.action.name == "Look")
    {
      DragCamera(ctx.ReadValue<Vector2>() * mouseDragSpeed);
    }
  }
  #endregion

  #region Control Methods
  /// <summary>
  /// Updates camera movement based on player input and map boundaries.
  /// </summary>
  /// <param name="deltaTime">Time passed since the last frame.</param>
  private void UpdateMovement(float deltaTime)
  {
    Vector3 forward = Camera.main.transform.forward;
    forward.y = 0f;
    forward.Normalize();

    Vector3 right = Camera.main.transform.right;
    right.y = 0f;
    right.Normalize();

    Vector3 inputVector = new Vector3(moveInput.x + edgeScrollInput.x, 0,
                                      moveInput.y + edgeScrollInput.y);

    if (inputVector.sqrMagnitude > 1f)
      inputVector.Normalize();

    float zoomMultiplier = moveSpeedZoomCurve.Evaluate(ZoomLevel);
    Vector3 targetVelocity = inputVector * moveSpeed * zoomMultiplier;

    if (inputVector.sqrMagnitude > 0.01f)
      Velocity = Vector3.MoveTowards(Velocity, targetVelocity, acceleration * deltaTime);
    else
      Velocity = Vector3.MoveTowards(Velocity, Vector3.zero, aeceleration * deltaTime);

    Vector3 motion = Velocity * deltaTime;
    cameraTarget.position += forward * motion.z + right * motion.x;

    // Clamp to map bounds if needed
    ClampToBounds();
  }

    /// <summary>
    /// Updates camera zoom smoothly based on scroll input.
    /// </summary>
    /// <param name="deltaTime">Time passed since the last frame.</param>
    private void UpdateZoom(float deltaTime)
    {
        if (Mathf.Abs(scrollInput.y) < 0.01f)
            return;

        float zoomChange = scrollInput.y * zoomSpeed;
        CurrentZoomSpeed = Mathf.Lerp(CurrentZoomSpeed, zoomChange, zoomSmoothing * deltaTime);

        float newZoom = orbitalFollow.RadialAxis.Value - CurrentZoomSpeed * deltaTime;
        orbitalFollow.RadialAxis.Value = Mathf.Clamp(newZoom, zoomClosest, zoomFurthest);
    }
    #endregion

    #region MonoBehaviour
    /// <summary>
    /// Initializes camera target position and map bounds.
    /// </summary>
    private void Start()
    {
        if (cameraTarget != null)
            cameraTarget.position += new Vector3(0, 2f, 0);
    }

    /// <summary>
    /// Moves the camera target based on mouse drag delta.
    /// </summary>
    private void DragCamera(Vector2 delta)
    {
        // Scale drag speed based on zoom level so movement feels natural
        float zoomMultiplier = moveSpeedZoomCurve.Evaluate(ZoomLevel);

        // Convert screen-space delta into world-space motion
        Vector3 right = Camera.main.transform.right;
        right.y = 0f;
        right.Normalize();

        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        // Apply delta (tweak multiplier for desired feel)
        float dragSpeed = 0.01f * moveSpeed * zoomMultiplier;
        Vector3 motion = (-right * delta.x + -forward * delta.y) * dragSpeed;

        cameraTarget.position += motion;

    // Clamp to map bounds if needed
    ClampToBounds();
  }

  private void ClampToBounds()
  {
    if (gridManager != null)
    {
        Vector3 pos = cameraTarget.position;
        pos.x = Mathf.Clamp(pos.x, gridManager.Boundary.Left - gridManager.Boundary.Width/2, gridManager.Boundary.Right - gridManager.Boundary.Width/2);
        pos.z = Mathf.Clamp(pos.z, gridManager.Boundary.Top - gridManager.Boundary.Height*2, gridManager.Boundary.Bottom - gridManager.Boundary.Height*2);
        cameraTarget.position = pos;
    }
  }

  /// <summary>
  /// Handles input and updates movement/zoom each frame.
  /// </summary>
  // TODO: The LateUpdate of CameraController should not contain any code under ANY circumstances.
  private void LateUpdate()
  {
    float deltaTime = Time.unscaledDeltaTime;

    if (Mouse.current != null)
    {
    }

    UpdateMovement(deltaTime);
    UpdateZoom(deltaTime);
  }

    #endregion
}
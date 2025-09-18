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
    [SerializeField] float MoveSpeed = 1000f;
    [SerializeField] float Acceleration = 10f;
    [SerializeField] float Deceleration = 10f;
    [SerializeField] AnimationCurve MoveSpeedZoomCurve = AnimationCurve.Linear(0f, 0.5f, 1f, 1f);

    [Header("Map")]
    [SerializeField] Transform Map; // Reference to your map object (e.g., Plane/Terrain).

    [Header("Zoom")]
    [SerializeField] float ZoomSpeed = 0.5f;
    [SerializeField] float ZoomSmoothing = 5f;
    [SerializeField] float ZoomClosest = 0.0f;
    [SerializeField] float ZoomFurthest = 10.0f;

    [Header("Components")]
    [SerializeField] Transform CameraTarget;
    [SerializeField] CinemachineOrbitalFollow OrbitalFollow;

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
    #endregion

    #region Properties
    /// <summary>
    /// Gets the current zoom level, normalized between 0 (zoomed in) and 1 (zoomed out).
    /// </summary>
    public float ZoomLevel
    {
        get
        {
            InputAxis axis = OrbitalFollow.RadialAxis;
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
    /// Handles middle mouse button input.
    /// </summary>
    public void OnMiddleClick(CallbackContext ctx) => middleClickInput = ctx.ReadValueAsButton();
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

        float zoomMultiplier = MoveSpeedZoomCurve.Evaluate(ZoomLevel);
        Vector3 targetVelocity = inputVector * MoveSpeed * zoomMultiplier;

        if (inputVector.sqrMagnitude > 0.01f)
            Velocity = Vector3.MoveTowards(Velocity, targetVelocity, Acceleration * deltaTime);
        else
            Velocity = Vector3.MoveTowards(Velocity, Vector3.zero, Deceleration * deltaTime);

        Vector3 motion = Velocity * deltaTime;
        CameraTarget.position += forward * motion.z + right * motion.x;

        if (Map != null)
        {
            Vector3 pos = CameraTarget.position;
            pos.x = Mathf.Clamp(pos.x, mapBounds.min.x, mapBounds.max.x);
            pos.z = Mathf.Clamp(pos.z, mapBounds.min.z, mapBounds.max.z);
            CameraTarget.position = pos;
        }
    }

    /// <summary>
    /// Updates camera zoom smoothly based on scroll input.
    /// </summary>
    /// <param name="deltaTime">Time passed since the last frame.</param>
    private void UpdateZoom(float deltaTime)
    {
        if (Mathf.Abs(scrollInput.y) < 0.01f)
            return;

        float zoomChange = scrollInput.y * ZoomSpeed;
        CurrentZoomSpeed = Mathf.Lerp(CurrentZoomSpeed, zoomChange, ZoomSmoothing * deltaTime);

        float newZoom = OrbitalFollow.RadialAxis.Value - CurrentZoomSpeed * deltaTime;
        OrbitalFollow.RadialAxis.Value = Mathf.Clamp(newZoom, ZoomClosest, ZoomFurthest);
    }
    #endregion

    #region MonoBehaviour
    /// <summary>
    /// Initializes camera target position and map bounds.
    /// </summary>
    private void Start()
    {
        if (CameraTarget != null)
            CameraTarget.position += new Vector3(0, 2f, 0);

        if (Map != null)
        {
            Vector3 size = Vector3.Scale(Map.localScale, new Vector3(10f, 1f, 10f));
            mapBounds = new Bounds(Map.position, size);
        }
    }

    /// <summary>
    /// Handles input and updates movement/zoom each frame.
    /// </summary>
    private void LateUpdate()
    {
        float deltaTime = Time.unscaledDeltaTime;

        if (Mouse.current != null)
        {
            if (Mouse.current.middleButton.wasPressedThisFrame)
                middleClickInput = true;

            if (Mouse.current.middleButton.wasReleasedThisFrame)
                middleClickInput = false;
        }

        UpdateMovement(deltaTime);
        UpdateZoom(deltaTime);
    }
    #endregion
}
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using Microsoft.Win32.SafeHandles;
using static UnityEngine.InputSystem.InputAction;

public class CameraController : MonoBehaviour
{
  #region Unity Editor Fields
  [Header("Movement")]
  [SerializeField] float MoveSpeed = 1000f;
  [SerializeField] float Acceleration = 10f;
  [SerializeField] float Deceleration = 10f;
  [SerializeField] AnimationCurve MoveSpeedZoomCurve = AnimationCurve.Linear(0f, 0.5f, 1f, 1f);

  [Header("Map")]
  [SerializeField] Transform Map; // dein Map-Objekt (Plane/Terrain etc.)

  [Header("Zoom")]
  [SerializeField] float ZoomSpeed = 0.5f;
  [SerializeField] float ZoomSmoothing = 5f;

  [Header("Components")]
  [SerializeField] Transform CameraTarget;
  [SerializeField] CinemachineOrbitalFollow OrbitalFollow;
  #endregion

  #region Misc Variables
  private InputSystem_Actions inputActions;
  private Vector2 moveInput;
  private Vector2 lookInput;
  private bool middleClickInput;
  Vector2 edgeScrollInput;

  float CurrentZoomSpeed = 0f;
  Vector3 Velocity = Vector3.zero;
  Bounds mapBounds;
  #endregion


  #region Properties
  public float ZoomLevel // value between 0 (zoomed in) and 1 (zoomed out)
  {
    get
    {
      InputAxis axis = OrbitalFollow.RadialAxis;
      return Mathf.InverseLerp(axis.Range.x, axis.Range.y, axis.Value);
    }
  }
  #endregion

  
  #region InputSystem EventHandlers
  public void OnMove(CallbackContext ctx)
  {
    moveInput = ctx.ReadValue<Vector2>();
  }

  public void OnLook(CallbackContext ctx)
  {
    lookInput = ctx.ReadValue<Vector2>();
  }

  public void OnScroll(CallbackContext ctx)
  {
    UpdateZoom(Time.deltaTime, ctx.ReadValue<Vector2>().y);
  }

  public void OnMiddleClick(CallbackContext ctx)
  {
    middleClickInput = ctx.ReadValueAsButton();
  }
  #endregion


  #region Control Methods
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

  private void UpdateZoom(float deltaTime, float scroll)
  {
    InputAxis axis = OrbitalFollow.RadialAxis;

    // Zielgeschwindigkeit nur setzen, wenn wirklich gescrollt wird
    float targetZoomSpeed = Mathf.Abs(scroll) > 0.01f ? ZoomSpeed * scroll : 0f;

    CurrentZoomSpeed = Mathf.Lerp(CurrentZoomSpeed, targetZoomSpeed, ZoomSmoothing * deltaTime);

    axis.Value -= CurrentZoomSpeed * deltaTime;
    OrbitalFollow.RadialAxis = axis;
  }
  #endregion


  #region MonoBehavior
  private void Start()
  {
    if (CameraTarget != null)
    {
      CameraTarget.position += new Vector3(0, 2f, 0);
    }

    if (Map != null)
    {
      // Berechne Bounds anhand von Position + Scale
      Vector3 size = Vector3.Scale(Map.localScale, new Vector3(10f, 1f, 10f));
      // Unity Plane ist 10x10, deshalb *10
      mapBounds = new Bounds(Map.position, size);
    }
  }

  private void LateUpdate()
  {
    float deltaTime = Time.unscaledDeltaTime;

    // Middle Mouse Button Handling
    if (Mouse.current != null)
    {
      if (Mouse.current.middleButton.wasPressedThisFrame)
        middleClickInput = true;

      if (Mouse.current.middleButton.wasReleasedThisFrame)
        middleClickInput = false;
    }

    UpdateMovement(deltaTime);
  }
  #endregion
}

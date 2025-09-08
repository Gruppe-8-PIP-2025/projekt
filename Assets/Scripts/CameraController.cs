using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace Zeus.RTSCamera
{
    public class CameraController : MonoBehaviour
    {
        private InputSystem_Actions inputActions;
        #region Input
        private Vector2 moveInput;
        private Vector2 lookInput;
        private bool middleClickInput = false;

        private void Awake()
        {
            inputActions = new InputSystem_Actions();

            inputActions.Player.ScrollWheel.performed += ctx =>
            {
                UpdateZoom(Time.deltaTime, ctx.ReadValue<Vector2>().y);
                Debug.Log(ctx.ReadValue<Vector2>().y);
            };
        }

        private void OnEnable()
        {
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            lookInput = value.Get<Vector2>();
        }
        #endregion

        [Header("Movement")]
        [SerializeField] float MoveSpeed = 1000f;

        [SerializeField] float Acceleration = 10f;
        [SerializeField] float Deceleration = 10f;

        Vector3 Velocity = Vector3.zero;

        [Header("Orbit")]
        [SerializeField] float OrbitSensitivity = 1f;
        [SerializeField] float OrbitSmoothing = 5f;

        [Header("Zoom")]
        [SerializeField] float ZoomSpeed = 0.5f;
        [SerializeField] float ZoomSmoothing = 5f;

        float CurrentZoomSpeed = 0f;

        [Header("Components")]
        [SerializeField] Transform CameraTarget;
        [SerializeField] CinemachineOrbitalFollow OrbitalFollow;

        #region Unity Methods
        private void Update()
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

            UpdateOrbit(deltaTime);
            UpdateMovement(deltaTime);
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

            Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y) * MoveSpeed;

            if (moveInput.sqrMagnitude > 0.01f)
                Velocity = Vector3.MoveTowards(Velocity, targetVelocity, Acceleration * deltaTime);
            else
                Velocity = Vector3.MoveTowards(Velocity, Vector3.zero, Deceleration * deltaTime);

            Vector3 motion = Velocity * deltaTime;
            CameraTarget.position += forward * motion.z + right * motion.x;
        }

        void UpdateOrbit(float deltaTime)
        {
            Vector2 orbitInput = lookInput * (middleClickInput ? 1f : 0f);
            orbitInput *= OrbitSensitivity;

            InputAxis horizontalAxis = OrbitalFollow.HorizontalAxis;
            InputAxis verticalAxis = OrbitalFollow.VerticalAxis;

           horizontalAxis.Value = Mathf.Lerp(horizontalAxis.Value, horizontalAxis.Value + orbitInput.x, OrbitSmoothing * deltaTime);
           // verticalAxis.Value = Mathf.Lerp(verticalAxis.Value, verticalAxis.Value - orbitInput.y, OrbitSmoothing * deltaTime);

            verticalAxis.Value = Mathf.Clamp(verticalAxis.Value, verticalAxis.Range.x, verticalAxis.Range.y);

            OrbitalFollow.HorizontalAxis = horizontalAxis;
            OrbitalFollow.VerticalAxis = verticalAxis;
        }

        void UpdateZoom(float deltaTime, float scroll)
        {
            InputAxis axis = OrbitalFollow.RadialAxis;

            // Zielgeschwindigkeit nur setzen, wenn wirklich gescrollt wird
            float targetZoomSpeed = Mathf.Abs(scroll) > 0.01f ? ZoomSpeed * scroll : 0f;

            CurrentZoomSpeed = Mathf.Lerp(CurrentZoomSpeed, targetZoomSpeed, ZoomSmoothing * deltaTime);

            axis.Value -= CurrentZoomSpeed * deltaTime;
            Debug.Log($"Axis value (unclamped): {axis.Value}");
            // axis.Value = Mathf.Clamp(axis.Value, axis.Range.x, axis.Range.y);
            // Debug.Log($"Axis value post-clamp: {axis.Value}");
            OrbitalFollow.RadialAxis = axis;
        }

        #endregion
    }
}

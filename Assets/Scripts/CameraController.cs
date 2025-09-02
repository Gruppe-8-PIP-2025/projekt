using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

namespace Zeus.RTSCamera
{
    public class CameraController : MonoBehaviour
    {
        #region Input
        private Vector2 moveInput;
        private Vector2 scrollInput;
        private Vector2 lookInput;
        bool middleClickInput = false;

        public void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            lookInput = value.Get<Vector2>();
        }

        public void OnScroll(InputValue value)
        {
            scrollInput = value.Get<Vector2>();
        }

        public void OnMiddleClick(InputValue value)
        {
            middleClickInput = value.isPressed;
        }

        #endregion

        [Header("Movement")]
        [SerializeField] float MoveSpeed = 1000f;

        [SerializeField] float Acceleration = 10f;
        [SerializeField] float Deceleration = 10f;

        Vector3 Velocity = Vector3.zero;


        [Header("Orbit")]
        [SerializeField] float OrbitSensitivity;
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

            UpdateOrbit(deltaTime);
            UpdateMovement(deltaTime);
            UpdateZoom(deltaTime);
        }
        #endregion

        #region Control Methods
        private void UpdateMovement(float deltaTime)
        { 

            // Kamera-Vorwärtsrichtung (Y = 0, nur horizontale Bewegung)
            Vector3 forward = Camera.main.transform.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = Camera.main.transform.right;
            right.y = 0f;
            right.Normalize(); 


            Vector3 targetVelocity = new Vector3(moveInput.x, 0, moveInput.y) * MoveSpeed;

            if (moveInput.sqrMagnitude > 0.01f)
            {
                Velocity = Vector3.MoveTowards(Velocity, targetVelocity, Acceleration * deltaTime);
            }
            else
            {
                Velocity = Vector3.MoveTowards(Velocity, Vector3.zero, Deceleration * deltaTime);
            }

                Vector3 motion = Velocity * deltaTime;

            CameraTarget.position += forward * motion.z + right * motion.x;
        }

        void UpdateOrbit(float deltaTime)
        {

            Vector2 orbitInput = lookInput * (middleClickInput ? 1f : 0f);

            orbitInput *= OrbitSensitivity;

            InputAxis horizontalAxis = OrbitalFollow.HorizontalAxis;
            InputAxis verticalAxis = OrbitalFollow.VerticalAxis;

            //horizontalAxis.Value += orbitInput.x;
            //verticalAxis.Value -= orbitInput.y;

            horizontalAxis.Value = Mathf.Lerp(horizontalAxis.Value, horizontalAxis.Value + orbitInput.x, OrbitSmoothing * deltaTime);
            verticalAxis.Value = Mathf.Lerp(verticalAxis.Value, verticalAxis.Value - orbitInput.y, OrbitSmoothing * deltaTime);

            //horizontalAxis.Value = Mathf.Clamp(horizontalAxis.Value, horizontalAxis.Range.x, horizontalAxis.Range.y);
            verticalAxis.Value = Mathf.Clamp(verticalAxis.Value, verticalAxis.Range.x, verticalAxis.Range.y);

           

            OrbitalFollow.HorizontalAxis = horizontalAxis;
            OrbitalFollow.VerticalAxis = verticalAxis;

        }

        void UpdateZoom(float deltaTime)
        {
            InputAxis axis = OrbitalFollow.RadialAxis;

            float targetZoomSpeed = 0f;

            if (Mathf.Abs(scrollInput.y) >= 0.01f)
            {
                targetZoomSpeed = ZoomSpeed * scrollInput.y;
            }

            CurrentZoomSpeed = Mathf.Lerp(CurrentZoomSpeed, targetZoomSpeed, ZoomSmoothing * deltaTime);

            axis.Value -= ZoomSpeed * scrollInput.y;
            axis.Value = Mathf.Clamp(axis.Value, axis.Range.x, axis.Range.y);

            OrbitalFollow.RadialAxis = axis;
        }
        #endregion
    }
}
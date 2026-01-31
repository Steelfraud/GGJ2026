using System;
using UnityEngine;

namespace Sampla.Player
{
    public class VehicleCameraController : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private Camera vehicleCamera;
        [SerializeField] private Transform cameraMoveTarget;
        [SerializeField] private Transform cameraLookTarget;
        [SerializeField] private bool staticCameraWorldUp = true;

        [Space]
        [SerializeField, Min(0f)] private float cameraMoveSpeed = 10f;
        [SerializeField, Min(0f)] private float cameraOffsetSpeed = 5f;
        [SerializeField, Min(0f)] private float cameraRotationSpeed = 10f;
        [SerializeField, Range(0f, 90f)] private float maxCameraLookAngle = 45f;
        [SerializeField] private AnimationCurve cameraHorizontalOffsetAtSteer = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(30f, 1f));
        //[SerializeField] private AnimationCurve cameraHorizontalOffsetAtDrift = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
        [SerializeField] private AnimationCurve cameraHorizontalOffsetMultiplierAtSpeed = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(100f, 1f));

        private float normalizedLook;
        private Vector3 currentMoveOffset;

        void Awake()
        {
            vehicleCamera.transform.SetParent(null);
        }

        void OnEnable()
        {
            vehicleController.PlayerInputController.OnLookInput += OnLookInputChanged;
        }

        void OnDisable()
        {
            vehicleController.PlayerInputController.OnLookInput -= OnLookInputChanged;
        }

        private void OnLookInputChanged(float inputValue)
        {
            normalizedLook = inputValue;
        }

        void LateUpdate()
        {
            UpdateCameraPosition();
            UpdateCameraRotation();
        }

        void UpdateCameraPosition()
        {
            Vector3 combinedCameraMoveTarget = cameraMoveTarget.transform.position;
            Vector3 offsetTarget = cameraMoveTarget.right 
            * cameraHorizontalOffsetAtSteer.Evaluate(Math.Abs(vehicleController.CurrentSteer)) 
            * cameraHorizontalOffsetMultiplierAtSpeed.Evaluate(vehicleController.CurrentSpeedKMH)
            * (vehicleController.CurrentDrift > 0 ? 1f : -1f);

            currentMoveOffset = Vector3.Lerp(currentMoveOffset, offsetTarget, cameraOffsetSpeed * Time.deltaTime);
            combinedCameraMoveTarget += currentMoveOffset;

            vehicleCamera.transform.position = Vector3.Lerp
            (
                vehicleCamera.transform.position, 
                combinedCameraMoveTarget, 
                cameraMoveSpeed * Time.deltaTime
            );
        }

        void UpdateCameraRotation()
        {
            Vector3 lookDirection = (cameraLookTarget.position - vehicleCamera.transform.position).normalized;
            Vector3 lookAxis = staticCameraWorldUp ? Vector3.up : Vector3.Cross(lookDirection, vehicleController.VehicleRigidbody.transform.right);

            Quaternion lookAheadRotation = Quaternion.LookRotation(lookDirection, lookAxis);
            Vector3 lookInputDirection = Quaternion.AngleAxis(maxCameraLookAngle * normalizedLook, lookAxis) * lookDirection;
            Quaternion combinedRotation = Quaternion.Slerp(lookAheadRotation, Quaternion.LookRotation(lookInputDirection, lookAxis), Mathf.Abs(normalizedLook));

            vehicleCamera.transform.rotation = Quaternion.Slerp
            (
                vehicleCamera.transform.rotation, 
                combinedRotation,
                cameraRotationSpeed * Time.deltaTime
            );
        }
    }
}
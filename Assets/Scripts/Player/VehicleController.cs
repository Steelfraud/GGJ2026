using System;
using UnityEngine;

namespace Sampla.Player
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private PlayerInputController playerInputController;
        [SerializeField] private Rigidbody vehicleRigidbody; public Rigidbody VehicleRigidbody { get { return vehicleRigidbody; } }

        [Space]
        [SerializeField, Min(0f)] private float torque = 100;
        [SerializeField, Min(0f)] private float maxTorque = 1000;
        [SerializeField, Min(0f)] private float downForce = 200;
        [SerializeField, Min(0f)] private float brakeTorque = 500;

        [Space]
        [SerializeField, Min(0f)] private float steerSpeed = 10f;
        [SerializeField, Min(0f)] private float steerMaxAngle = 25f;


        [Space]
        [SerializeField] private Transform throttleCenter;
        [SerializeField] private Transform downForceCenter;
        [SerializeField] private Transform frontSteeringCenter;
        [SerializeField] private Transform backSteeringCenter;
        [SerializeField] private Transform centerOfMass;

        [Space]
        [SerializeField] private WheelCollider wheelFrontLeft;
        [SerializeField] private WheelCollider wheelFrontRight;
        [SerializeField] private WheelCollider wheelBackLeft;
        [SerializeField] private WheelCollider wheelBackRight;

        [SerializeField] private Transform wheelModelFrontLeft;
        [SerializeField] private Transform wheelModelFrontRight;
        [SerializeField] private Transform wheelModelBackLeft;
        [SerializeField] private Transform wheelModelBackRight;

        private float currentSteer;
        private float normalizedSteer;
        private float normalizedTorque;
        private float normalizedBrake;

        void OnEnable()
        {
            playerInputController.OnSteerInput += OnSteerInputChanged;
            playerInputController.OnThrottleInput += OnThrottleInputChanged;
            playerInputController.OnBrakeInput += OnBrakeInputChanged;

            CenterOfMassUpdate();
        }

        void OnDisable()
        {
            playerInputController.OnSteerInput -= OnSteerInputChanged;
            playerInputController.OnThrottleInput -= OnThrottleInputChanged;
            playerInputController.OnBrakeInput -= OnBrakeInputChanged;
        }

        void OnThrottleInputChanged(float inputValue)
        {
            normalizedTorque = inputValue;
        }

        void OnSteerInputChanged(float inputValue)
        {
            normalizedSteer = inputValue;
        }

        void OnBrakeInputChanged(float inputValue)
        {
            normalizedBrake = inputValue;
        }

        void FixedUpdate()
        {
            //DownForceUpdate();
            MotorTorqueUpdate();
            SteeringUpdate();
            BreakUpdate();
        }

        void Update()
        {
            UpdateWheels();
        }

        void CenterOfMassUpdate()
        {
            if (vehicleRigidbody.centerOfMass == vehicleRigidbody.transform.InverseTransformPoint(centerOfMass.position))
                return;

            vehicleRigidbody.centerOfMass = vehicleRigidbody.transform.InverseTransformPoint(centerOfMass.position);
            Debug.Log("Center Of Mass Updated: " + vehicleRigidbody.worldCenterOfMass);
        }

        void DownForceUpdate()
        {
            //vehicleRigidbody.AddForceAtPosition(-downForceCenter.up * downForce, downForceCenter.transform.position);
        }

        void MotorTorqueUpdate()
        {
            if (normalizedTorque > 0)
            {
                wheelFrontLeft.motorTorque = Mathf.Min(wheelFrontLeft.motorTorque + (torque * normalizedTorque), maxTorque);
                wheelFrontRight.motorTorque = Mathf.Min(wheelFrontRight.motorTorque + (torque * normalizedTorque), maxTorque);
            }
            else
            {
                wheelFrontLeft.motorTorque = Mathf.Max(wheelFrontLeft.motorTorque - torque, 0);
                wheelFrontRight.motorTorque = Mathf.Max(wheelFrontRight.motorTorque - torque, 0);
            }

            //Debug.Log("Left: " + wheelFrontLeft.rpm + " | Right: " + wheelFrontRight.rpm);


            // wheelFrontLeft.motorTorque = currentTorque;
            // wheelFrontRight.motorTorque += torque * normalizedThrottle;
            //vehicleRigidbody.AddForceAtPosition(throttleCenter.forward * throttleForce * normalizedThrottle, throttleCenter.position);
        }

        void SteeringUpdate()
        {
            currentSteer = Mathf.Lerp(currentSteer, normalizedSteer * steerMaxAngle, steerSpeed * Time.fixedDeltaTime);

            wheelFrontLeft.steerAngle = currentSteer;
            wheelFrontRight.steerAngle = currentSteer;

            //throttleCenter.localRotation = Quaternion.AngleAxis(steerMaxAngle * -normalizedSteer, throttleCenter.up);
            //vehicleRigidbody.AddForceAtPosition(frontSteeringCenter.right * steeringForce * normalizedSteer * frontSteeringMultiplier, frontSteeringCenter.position);
            //vehicleRigidbody.AddForceAtPosition(-backSteeringCenter.right * steeringForce * normalizedSteer * backSteeringMultiplier, backSteeringCenter.position);
            //vehicleRigidbody.AddRelativeTorque(frontSteeringCenter.up * steeringForce * normalizedSteer);
        }

        void BreakUpdate()
        {
            wheelFrontLeft.brakeTorque = brakeTorque * normalizedBrake;
            wheelFrontRight.brakeTorque = brakeTorque * normalizedBrake;
            wheelBackLeft.brakeTorque = brakeTorque * normalizedBrake;
            wheelBackRight.brakeTorque = brakeTorque * normalizedBrake;
        }

        void UpdateWheels()
        {
            UpdateWheel(wheelFrontLeft, wheelModelFrontLeft);
            UpdateWheel(wheelFrontRight, wheelModelFrontRight);
            UpdateWheel(wheelBackLeft, wheelModelBackLeft);
            UpdateWheel(wheelBackRight, wheelModelBackRight);
        }

        void UpdateWheel(WheelCollider wheelCollider, Transform wheelModel)
        {
            wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            wheelModel.position = pos;
            wheelModel.rotation = rot;
        }

        public float GetAverageFrontWheelsRPM()
        {
            float averageRPM = 0;
            averageRPM += wheelFrontLeft.rpm;
            averageRPM += wheelFrontRight.rpm;
            averageRPM /= 2;
            return averageRPM;
        }

        void OnDrawGizmos()
        {
            if (!Application.isPlaying || vehicleRigidbody == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(vehicleRigidbody.worldCenterOfMass, 0.1f);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sampla.Player
{
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private PlayerInputController playerInputController; public PlayerInputController PlayerInputController { get { return playerInputController; } }
        [SerializeField] private Rigidbody vehicleRigidbody; public Rigidbody VehicleRigidbody { get { return vehicleRigidbody; } }

        [Space]
        [SerializeField, Min(0f)] private float torque = 100;
        [SerializeField, Min(0f)] private float maxTorque = 1000;
        [SerializeField, Min(0f)] private float reverseThresholdInKMH = 5;
        [SerializeField, Min(0f)] private float reverseTorque = 50;
        [SerializeField, Min(0f)] private float maxReverseTorque = 500;
        [SerializeField, Min(0f)] private float downForce = 200;
        [SerializeField, Min(0f)] private float brakeTorque = 500;
        [SerializeField, Min(0f)] private float maxSpeedKMH = 300;

        [Space]
        [SerializeField, Min(0f)] private float steerSpeed = 10f;
        [FormerlySerializedAs("steerSpeedCurve")] [SerializeField] private AnimationCurve speedCurve;
        [SerializeField, Min(0f)] private float steerMaxAngle = 25f;


        [Space]
        //[SerializeField] private Transform throttleCenter;
        //[SerializeField] private Transform downForceCenter;
        //[SerializeField] private Transform frontSteeringCenter;
        //[SerializeField] private Transform backSteeringCenter;
        [SerializeField] private Transform centerOfMass;

        [Space]
        [SerializeField] private WheelCollider wheelFrontLeft; public WheelCollider WheelFrontLeft { get { return wheelFrontLeft; } }
        [SerializeField] private WheelCollider wheelFrontRight; public WheelCollider WheelFrontRight { get { return wheelFrontRight; } }
        [SerializeField] private WheelCollider wheelBackLeft; public WheelCollider WheelBackLeft { get { return wheelBackLeft; } }
        [SerializeField] private WheelCollider wheelBackRight; public WheelCollider WheelBackRight { get { return wheelBackRight; } }

        [Space]
        [SerializeField] private Transform wheelModelFrontLeft;
        [SerializeField] private Transform wheelModelFrontRight;
        [SerializeField] private Transform wheelModelBackLeft;
        [SerializeField] private Transform wheelModelBackRight;

        private float currentSpeedMS; public float CurrentSpeedMS { get { return currentSpeedMS; } }
        private float currentSpeedKMH; public float CurrentSpeedKMH { get { return currentSpeedKMH; } }
        private float currentDrift; public float CurrentDrift { get { return currentDrift; } }
        private float currentSteer; public float CurrentSteer { get { return currentSteer; } }
        private Vector3 currentVelocityDirection; public Vector3 CurrentVelocityDirection { get { return currentVelocityDirection; } }

        private float normalizedSteer;
        private float normalizedTorque;
        private float normalizedBrake;

        void OnEnable()
        {
            playerInputController.OnSteerInput += OnSteerInputChanged;
            playerInputController.OnThrottleInput += OnThrottleInputChanged;
            playerInputController.OnBrakeInput += OnBrakeInputChanged;
            vehicleRigidbody.maxLinearVelocity = KMHToMS(maxSpeedKMH);
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
            DownForceUpdate();
            MotorTorqueUpdate();
            SteeringUpdate();
            BreakUpdate();
            CacheProperties();
        }

        void Update()
        {
            UpdateWheels();
        }

        void CacheProperties()
        {
            currentSpeedMS = vehicleRigidbody.linearVelocity.magnitude;
            currentSpeedKMH = MSToKMH(currentSpeedMS);
            currentVelocityDirection = vehicleRigidbody.linearVelocity.normalized;
            currentDrift = Vector3.SignedAngle(currentVelocityDirection, vehicleRigidbody.transform.forward, vehicleRigidbody.transform.up);
        }

        void CenterOfMassUpdate()
        {
            if (vehicleRigidbody.centerOfMass == vehicleRigidbody.transform.InverseTransformPoint(centerOfMass.position))
                return;

            vehicleRigidbody.centerOfMass = vehicleRigidbody.transform.InverseTransformPoint(centerOfMass.position);
            // Debug.Log("Center Of Mass Updated: " + vehicleRigidbody.worldCenterOfMass);
        }

        void DownForceUpdate()
        {
            var eval = 1 - (currentSpeedKMH / maxSpeedKMH);
            var downForceEval = speedCurve.Evaluate(eval) * downForce;
            vehicleRigidbody.AddForceAtPosition(-transform.up * downForceEval, vehicleRigidbody.position);
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
            var steerSpeedEval = speedCurve.Evaluate(currentSpeedKMH / maxSpeedKMH) * steerSpeed;
            currentSteer = Mathf.Lerp(currentSteer, normalizedSteer * steerMaxAngle, steerSpeedEval * Time.fixedDeltaTime);

            wheelFrontLeft.steerAngle = currentSteer;
            wheelFrontRight.steerAngle = currentSteer;

            //throttleCenter.localRotation = Quaternion.AngleAxis(steerMaxAngle * -normalizedSteer, throttleCenter.up);
            //vehicleRigidbody.AddForceAtPosition(frontSteeringCenter.right * steeringForce * normalizedSteer * frontSteeringMultiplier, frontSteeringCenter.position);
            //vehicleRigidbody.AddForceAtPosition(-backSteeringCenter.right * steeringForce * normalizedSteer * backSteeringMultiplier, backSteeringCenter.position);
            //vehicleRigidbody.AddRelativeTorque(frontSteeringCenter.up * steeringForce * normalizedSteer);
        }

        void BreakUpdate()
        {
            if (normalizedBrake == 0)
            {
                wheelFrontLeft.brakeTorque = 0f;
                wheelFrontRight.brakeTorque = 0f;
                wheelBackLeft.brakeTorque = 0f;
                wheelBackRight.brakeTorque = 0f;
                return;
            }

            if (vehicleRigidbody.linearVelocity.magnitude > VehicleController.KMHToMS(reverseThresholdInKMH) && Vector3.Dot(vehicleRigidbody.linearVelocity.normalized, transform.forward) > 0)
            {
                wheelFrontLeft.brakeTorque = brakeTorque * normalizedBrake;
                wheelFrontRight.brakeTorque = brakeTorque * normalizedBrake;
                wheelBackLeft.brakeTorque = brakeTorque * normalizedBrake;
                wheelBackRight.brakeTorque = brakeTorque * normalizedBrake;
            }
            else
            {
                wheelFrontLeft.brakeTorque = 0f;
                wheelFrontRight.brakeTorque = 0f;
                wheelBackLeft.brakeTorque = 0f;
                wheelBackRight.brakeTorque = 0f;

                wheelFrontLeft.motorTorque = Mathf.Max(wheelFrontLeft.motorTorque - (reverseTorque * normalizedBrake), -maxReverseTorque);
                wheelFrontRight.motorTorque = Mathf.Max(wheelFrontLeft.motorTorque - (reverseTorque * normalizedBrake), -maxReverseTorque);
                wheelBackLeft.motorTorque = Mathf.Max(wheelFrontLeft.motorTorque - (reverseTorque * normalizedBrake), -maxReverseTorque);
                wheelBackRight.motorTorque = Mathf.Max(wheelFrontLeft.motorTorque - (reverseTorque * normalizedBrake), -maxReverseTorque);
            }

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

        public static float MSToKMH(float metersPerSecond)
        {
            return metersPerSecond * 3.6f;
        }

        public static float KMHToMS(float kilometersPerHour)
        {
            return kilometersPerHour * 0.2777777778f;
        }
    }
}
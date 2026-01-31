using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sampla.Player
{
    public class VehicleController : MonoBehaviour
    {
        public delegate void ValueChangedAction(bool value);
        public event ValueChangedAction OnTurboChange;

        [SerializeField] private PlayerInputController playerInputController; public PlayerInputController PlayerInputController { get { return playerInputController; } }
        [SerializeField] private Rigidbody vehicleRigidbody; public Rigidbody VehicleRigidbody { get { return vehicleRigidbody; } }

        [Space]
        [SerializeField, Min(0f)] private float torque = 100;
        [SerializeField, Min(0f)] private float maxTorque = 1000;
        [SerializeField, Min(0f)] private float reverseThresholdInKMH = 5;
        [SerializeField, Min(0f)] private float reverseTorque = 50;
        [SerializeField, Min(0f)] private float maxReverseTorque = 500;
        [SerializeField, Min(0f)] private float downForce = 200;
        [SerializeField] private AnimationCurve downForceCurve;
        [SerializeField, Min(0f)] private float brakeTorque = 500;
        [SerializeField, Min(0f)] private float maxSpeedKMH = 300;
        [SerializeField, Min(0f)] private float turboForce = 1000;
        [SerializeField, Min(0f)] private float maxTurboTime = 2f;
        [SerializeField] private AnimationCurve linearDampingCurve;

        [Space]
        [SerializeField, Min(0f)] private float steerSpeed = 10f;
        [SerializeField] private AnimationCurve steerSpeedCurve;
        [SerializeField, Min(0f)] private float steerMaxAngle = 25f;


        [Space]
        //[SerializeField] private Transform throttleCenter;
        //[SerializeField] private Transform downForceCenter;
        //[SerializeField] private Transform frontSteeringCenter;
        //[SerializeField] private Transform backSteeringCenter;
        [SerializeField] private Transform centerOfMass;
        [SerializeField] private Transform turboCenter;

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

        private float currentSpeedLerp; public float CurrentSpeedLerp { get { return currentSpeedLerp; } }
        private float currentSpeedMS; public float CurrentSpeedMS { get { return currentSpeedMS; } }
        private float currentSpeedKMH; public float CurrentSpeedKMH { get { return currentSpeedKMH; } }
        private float currentDrift; public float CurrentDrift { get { return currentDrift; } }
        private float currentSteer; public float CurrentSteer { get { return currentSteer; } }
        private float currentTurboTimeLeft; public float CurrentTurboTimeLeft { get { return currentTurboTimeLeft; } }
        private Vector3 currentVelocityDirection; public Vector3 CurrentVelocityDirection { get { return currentVelocityDirection; } }

        private float normalizedSteer;
        private float normalizedTorque;
        private float normalizedBrake;
        private float normalizedTurbo;

        private bool isTurboing;

        void OnValidate()
        {
            //vehicleRigidbody.maxLinearVelocity = KMHToMS(maxSpeedKMH);
        }

        void OnEnable()
        {
            playerInputController.OnSteerInput += OnSteerInputChanged;
            playerInputController.OnThrottleInput += OnThrottleInputChanged;
            playerInputController.OnBrakeInput += OnBrakeInputChanged;
            playerInputController.OnTurboInput += OnTurboInputChanged;
            // vehicleRigidbody.maxLinearVelocity = KMHToMS(maxSpeedKMH);
            currentTurboTimeLeft = maxTurboTime;
            CenterOfMassUpdate();
        }

        void OnDisable()
        {
            playerInputController.OnSteerInput -= OnSteerInputChanged;
            playerInputController.OnThrottleInput -= OnThrottleInputChanged;
            playerInputController.OnBrakeInput -= OnBrakeInputChanged;
            playerInputController.OnTurboInput -= OnTurboInputChanged;
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

        void OnTurboInputChanged(float inputValue)
        {
            normalizedTurbo = inputValue;
        }

        void FixedUpdate()
        {
            currentSpeedLerp = currentSpeedKMH / maxSpeedKMH;

            if (vehicleRigidbody.isKinematic)
            {
                return;
            }            

            DownForceUpdate();
            MotorTorqueUpdate();
            SteeringUpdate();
            BreakUpdate();
            TurboUpdate();
            LinearDampingUpdate();
            CacheProperties();
            //Debug.Log($"{vehicleRigidbody.linearVelocity.magnitude}, {vehicleRigidbody.linearDamping}");
        }

        void LinearDampingUpdate()
        {
            vehicleRigidbody.linearDamping = linearDampingCurve.Evaluate(CurrentSpeedLerp);
        }

        void Update()
        {
            UpdateWheels();
        }

        public void ResetVehicle()
        {
            vehicleRigidbody.linearVelocity = Vector3.zero;
            vehicleRigidbody.angularVelocity = Vector3.zero;

            wheelBackLeft.motorTorque = 0;
            wheelBackLeft.brakeTorque = 0;
            wheelBackLeft.steerAngle = 0;

            wheelBackRight.motorTorque = 0;
            wheelBackRight.brakeTorque = 0;
            wheelBackRight.steerAngle = 0;

            wheelFrontLeft.motorTorque = 0;
            wheelFrontLeft.brakeTorque = 0;
            wheelFrontLeft.steerAngle = 0;

            wheelFrontRight.motorTorque = 0;
            wheelFrontRight.brakeTorque = 0;
            wheelFrontRight.steerAngle = 0;

            vehicleRigidbody.isKinematic = true;
        }

        public void RestartVehicle()
        {
            vehicleRigidbody.isKinematic = false;

            vehicleRigidbody.linearVelocity = transform.forward * 20;
            wheelBackLeft.motorTorque = 250;
            wheelBackRight.motorTorque = 250;
            wheelFrontLeft.motorTorque = 250;
            wheelFrontRight.motorTorque = 250;
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
            var downForceEval = downForceCurve.Evaluate(CurrentSpeedLerp) * downForce;
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
            var speedEval = steerSpeedCurve.Evaluate(CurrentSpeedLerp) * steerSpeed;
            if (Math.Abs(normalizedSteer) < Math.Abs(currentSteer))
            {
                currentSteer = Mathf.Lerp(currentSteer, normalizedSteer * steerMaxAngle, speedEval * 10 * Time.fixedDeltaTime);
            }
            else
            {
                currentSteer = Mathf.Lerp(currentSteer, normalizedSteer * steerMaxAngle, speedEval * Time.fixedDeltaTime);
            }

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

        void TurboUpdate()
        {
            if (normalizedTurbo > 0)
            {
                currentTurboTimeLeft = Mathf.Max(currentTurboTimeLeft - Time.fixedDeltaTime, 0f);
            }
            else
            {
                currentTurboTimeLeft = Mathf.Min(currentTurboTimeLeft + Time.fixedDeltaTime, maxTurboTime);
            }

            float turboForceMagnitude = currentTurboTimeLeft > 0 ? turboForce * normalizedTurbo : 0f;

            if (turboForceMagnitude > 0 && !isTurboing)
            {
                isTurboing = true;
                OnTurboChange?.Invoke(true);
            }
            else if (turboForceMagnitude == 0 && isTurboing)
            {
                isTurboing = false;
                OnTurboChange?.Invoke(false);
            }

            vehicleRigidbody.AddForceAtPosition(turboCenter.forward * turboForceMagnitude, turboCenter.position);
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
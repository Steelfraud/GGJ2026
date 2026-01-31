using UnityEngine;

namespace Sampla.Player
{
    public class VehicleEffects : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;

        [SerializeField, Range(0f, 90)] private float driftAngle = 20f;
        [SerializeField, Min(0f)] private float driftSpeed = 20f;
        [SerializeField, Min(0f)] private float driftRPM = 400;
        [SerializeField] private ParticleSystem frontLeftWheelDriftParticles;
        [SerializeField] private ParticleSystem frontRightWheelDriftParticles;

        void Update()
        {
            UpdateWheelParticles();
        }

        void UpdateWheelParticles()
        {
            Debug.DrawRay(vehicleController.transform.position, vehicleController.VehicleRigidbody.linearVelocity.normalized * 10f, Color.red);
            Debug.DrawRay(vehicleController.transform.position, vehicleController.transform.forward * 10f, Color.green);
            float vehicleAngleToVelocity = Vector3.Angle(vehicleController.VehicleRigidbody.linearVelocity.normalized, vehicleController.transform.forward);

            //Debug.Log(vehicleAngleToVelocity);

            if (vehicleAngleToVelocity > driftAngle)
            {
                //if (Mathf.Abs(vehicleController.WheelFrontLeft.rpm) > driftRPM)
                if (vehicleController.CurrentSpeedKMH > driftSpeed)
                {
                    if (frontLeftWheelDriftParticles != null && !frontLeftWheelDriftParticles.isPlaying)
                    {
                        frontLeftWheelDriftParticles.Play();
                    }
                }
                else
                {
                    frontLeftWheelDriftParticles?.Stop();
                }
                //if (Mathf.Abs(vehicleController.WheelFrontRight.rpm) > driftRPM)
                if (vehicleController.CurrentSpeedKMH > driftSpeed)
                {
                    if (frontRightWheelDriftParticles != null && !frontRightWheelDriftParticles.isPlaying)
                    {
                        frontRightWheelDriftParticles?.Play();
                    }
                }
                else
                {
                    frontRightWheelDriftParticles?.Stop();
                }
            }
            else
            {
                frontLeftWheelDriftParticles?.Stop();
                frontRightWheelDriftParticles?.Stop();
            }
        }
    }
}
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
            Debug.DrawRay(vehicleController.transform.position, vehicleController.CurrentVelocityDirection * 10f, Color.red);
            Debug.DrawRay(vehicleController.transform.position, vehicleController.transform.forward * 10f, Color.green);

            if (vehicleController.CurrentDrift > driftAngle)
            {
                if (Mathf.Abs(vehicleController.WheelFrontLeft.rpm) > driftRPM)
                //if (vehicleController.CurrentSpeedKMH > driftSpeed)
                {
                    PlayParticles(frontLeftWheelDriftParticles);
                }
                else
                {
                    StopParticles(frontLeftWheelDriftParticles);
                }
                if (Mathf.Abs(vehicleController.WheelFrontRight.rpm) > driftRPM)
                //if (vehicleController.CurrentSpeedKMH > driftSpeed)
                {
                    PlayParticles(frontRightWheelDriftParticles);
                }
                else
                {
                    StopParticles(frontRightWheelDriftParticles);
                }
            }
            else
            {
                StopParticles(frontLeftWheelDriftParticles);
                StopParticles(frontRightWheelDriftParticles);
            }
        }

        void PlayParticles(ParticleSystem particleSystem)
        {
            if (particleSystem == null || particleSystem.isEmitting)
                return;

            particleSystem.Play();
            //Debug.Log("Play particles: " + particleSystem.gameObject.name);
        }

        void StopParticles(ParticleSystem particleSystem)
        {
            if (particleSystem == null || !particleSystem.isEmitting /*|| !particleSystem.IsAlive(withChildren: true)*/)
                return;

            particleSystem.Stop();
            //Debug.Log("Stop particles: " + particleSystem.gameObject.name);
        }
    }
}
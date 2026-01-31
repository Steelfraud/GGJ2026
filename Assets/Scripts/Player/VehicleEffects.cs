using UnityEngine;

namespace Sampla.Player
{
    public class VehicleEffects : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;

        [SerializeField, Range(0f, 90)] private float driftAngle = 20f;
        [SerializeField] private ParticleSystem backLeftWheelDriftParticles;
        [SerializeField] private ParticleSystem backRightWheelDriftParticles;

        void Update()
        {
            UpdateWheelParticles();
        }

        void UpdateWheelParticles()
        {
            float vehicleAngleToVelocity = Vector3.Angle(vehicleController.VehicleRigidbody.linearVelocity.normalized, vehicleController.transform.forward);

            Debug.Log(vehicleAngleToVelocity);

            if (vehicleController.WheelBackLeft.isGrounded && vehicleAngleToVelocity > driftAngle)
            {
                backLeftWheelDriftParticles?.Play();
            }
            else
            {
                backLeftWheelDriftParticles?.Stop();
            }

            if (vehicleController.WheelBackRight.isGrounded && vehicleAngleToVelocity > driftAngle)
            {
                backRightWheelDriftParticles?.Play();
            }
            else
            {
                backRightWheelDriftParticles?.Stop();
            }
        }
    }
}
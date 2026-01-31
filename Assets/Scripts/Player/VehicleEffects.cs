using System;
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

        [SerializeField] private TrailRenderer[] turboTrailRenderers;

        void OnEnable()
        {
            OnTurboChanged(isTurboing: false);
            vehicleController.OnTurboChange += OnTurboChanged;
        }

        void OnDisable()
        {
            vehicleController.OnTurboChange -= OnTurboChanged;
        }

        void Update()
        {
            UpdateWheelParticles();
        }

        void UpdateWheelParticles()
        {
            Debug.DrawRay(vehicleController.transform.position, vehicleController.CurrentVelocityDirection * 10f, Color.red);
            Debug.DrawRay(vehicleController.transform.position, vehicleController.transform.forward * 10f, Color.green);

            if (Mathf.Abs(vehicleController.CurrentDrift) > driftAngle)
            {
                if (Mathf.Abs(vehicleController.WheelFrontLeft.rpm) > driftRPM && vehicleController.WheelFrontLeft.isGrounded)
                {
                    PlayParticles(frontLeftWheelDriftParticles);
                }
                else
                {
                    StopParticles(frontLeftWheelDriftParticles);
                }
                if (Mathf.Abs(vehicleController.WheelFrontRight.rpm) > driftRPM && vehicleController.WheelFrontRight.isGrounded)
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

        void OnTurboChanged(bool isTurboing)
        {
            if (turboTrailRenderers == null)
                return;

            for (int i = 0; i < turboTrailRenderers.Length; i++)
            {
                if (turboTrailRenderers[i] == null)
                    continue;

                turboTrailRenderers[i].emitting = isTurboing;
                // if (isTurboing)
                // {
                //     //PlayParticles(turboParticles[i]);
                // }
                // else
                // {
                //     turboTrailRenderers[i].SetActive(false);
                //     //StopParticles(turboParticles[i]);
                // }
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
using UnityEngine;
using UnityEngine.InputSystem;

namespace Sampla.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        public delegate void PlayerInputAction(float inputValue);
        public event PlayerInputAction OnSteerInput;
        public event PlayerInputAction OnThrottleInput;
        public event PlayerInputAction OnBrakeInput;

        [HideInInspector, SerializeField] private PlayerInput playerInput;

        private float steerInput;
        private float throttleInput;
        private float brakeInput;

        void Oalidate()
        {
            if (TryGetComponent(out PlayerInput playerInput))
            {
                this.playerInput = playerInput;
            }            
        }

        void OnSteer(InputValue input)
        {
            steerInput = input.Get<float>();
            //Debug.Log("Steer: " + steerInput);
            OnSteerInput?.Invoke(steerInput);
        }

        void OnThrottle(InputValue input)
        {
            throttleInput = input.Get<float>();
            //Debug.Log("Throttle: " + throttleInput);
            OnThrottleInput?.Invoke(throttleInput);
        }

        void OnBrake(InputValue input)
        {
            brakeInput = input.Get<float>();
            //Debug.Log("Brake: " + brakeInput);
            OnBrakeInput?.Invoke(brakeInput);
        }
    }
}
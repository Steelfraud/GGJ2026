using System;
using UnityEngine;

namespace Sampla.Player
{
    public class PlayerAvatarIKController : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private Transform headTarget;
        [SerializeField] private AnimationCurve headHorizontalMoveAtSteer = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(30f, 1f));
        [SerializeField] private AnimationCurve headTiltAtSteer = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(30f, 20f));
        [SerializeField, Min(0f)] private float headTargetFollowSpeed = 1f; 
        [SerializeField] private bool invertHeadDirection;

        [Space]
        [SerializeField, Min(0f)] private float headTargetSideTiltSpeed = 1f;
        [SerializeField, Min(0f)] private float headTargetTurboTiltSpeed = 1f;
        [SerializeField, Min(0f)] private float headTiltAtTurbo = 30f;

        private Vector3 defaultHeadTargetPosition;
        private Vector3 defaultHeadRightDirection;

        private bool isTurboing;
        private float currentSideTilt;
        private float currentTurboTilt;

        void Awake()
        {
            defaultHeadTargetPosition = headTarget.localPosition;
            defaultHeadRightDirection = headTarget.right;
        }

        void OnEnable()
        {
            vehicleController.OnTurboChange += OnTurboChanged;
        }

        void OnDisable()
        {
            vehicleController.OnTurboChange -= OnTurboChanged;
        }

        void OnTurboChanged(bool value)
        {
            isTurboing = value;
        }

        void Update()
        {
            Vector3 currentOffset = defaultHeadRightDirection * headHorizontalMoveAtSteer.Evaluate(Mathf.Abs(vehicleController.CurrentSteer));
            currentOffset *= vehicleController.CurrentSteer > 0 ? 1 : -1;
            currentOffset *= invertHeadDirection ? -1 : 1;
            Vector3 targetPosition = defaultHeadTargetPosition + currentOffset;
            headTarget.localPosition = Vector3.Lerp(headTarget.localPosition, targetPosition, headTargetFollowSpeed * Time.deltaTime);

            float sideTilt = headTiltAtSteer.Evaluate(Mathf.Abs(vehicleController.CurrentSteer));
            sideTilt *= vehicleController.CurrentSteer > 0 ? 1 : -1;
            sideTilt *= invertHeadDirection ? 1 : -1;
            currentSideTilt = Mathf.Lerp(currentSideTilt, sideTilt, headTargetSideTiltSpeed * Time.deltaTime);

            currentTurboTilt = Mathf.Lerp(currentTurboTilt, isTurboing ? -headTiltAtTurbo : 0, headTargetTurboTiltSpeed * Time.deltaTime);

            Vector3 eulerTarget = new Vector3(currentTurboTilt, 0f, currentSideTilt);
            headTarget.localRotation = Quaternion.Euler(eulerTarget);
            //headTarget.localRotation = Quaternion.Slerp(headTarget.localRotation, Quaternion.Euler(eulerTarget), headTargetSideTiltSpeed * Time.deltaTime);
        }
    }
}
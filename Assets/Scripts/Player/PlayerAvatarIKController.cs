using UnityEngine;

namespace Sampla.Player
{
    public class PlayerAvatarIKController : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private Transform headTarget;
        [SerializeField] private AnimationCurve headHorizontalMoveAtSteer = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(30f, 1f));
        [SerializeField, Min(0f)] private float headTargetFollowSpeed = 1f; 
        [SerializeField] private bool invertHeadDirection;

        private Vector3 defaultHeadTargetPosition;
        private Vector3 defaultHeadRightDirection;

        void Awake()
        {
            defaultHeadTargetPosition = headTarget.localPosition;
            defaultHeadRightDirection = headTarget.right;
        }

        void Update()
        {
            Vector3 currentOffset = defaultHeadRightDirection * headHorizontalMoveAtSteer.Evaluate(Mathf.Abs(vehicleController.CurrentSteer));
            currentOffset *= vehicleController.CurrentSteer > 0 ? 1 : -1;
            currentOffset *= invertHeadDirection ? -1 : 1;
            Vector3 targetPosition = defaultHeadTargetPosition + currentOffset;
            headTarget.localPosition = Vector3.Lerp(headTarget.localPosition, targetPosition, headTargetFollowSpeed * Time.deltaTime);
        }
    }
}
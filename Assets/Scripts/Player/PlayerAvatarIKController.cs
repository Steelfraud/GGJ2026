using UnityEngine;

namespace Sampla.Player
{
    public class PlayerAvatarIKController : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private Transform headTarget;

        private Vector3 defaultHeadTargetPosition;

        void Awake()
        {
            defaultHeadTargetPosition = headTarget.localPosition;
        }

        void Update()
        {
            
        }
    }
}
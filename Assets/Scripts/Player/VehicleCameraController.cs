using UnityEngine;

namespace Sampla.Player
{
    public class VehicleCameraController : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private Camera vehicleCamera;
        [SerializeField] private Transform cameraMoveTarget;
        [SerializeField] private Transform cameraLookTarget;
        [SerializeField, Min(0f)] private float cameraMoveSpeed = 10f;
        [SerializeField, Min(0f)] private float cameraRotationSpeed = 10f;

        void Awake()
        {
            vehicleCamera.transform.SetParent(null);
        }

        void Update()
        {
            vehicleCamera.transform.position = Vector3.Lerp
            (
                vehicleCamera.transform.position, 
                cameraMoveTarget.transform.position, 
                cameraMoveSpeed * Time.deltaTime
            );
            vehicleCamera.transform.rotation = Quaternion.Slerp
            (
                vehicleCamera.transform.rotation, 
                Quaternion.LookRotation((cameraLookTarget.position - vehicleCamera.transform.position).normalized),
                //Quaternion.LookRotation(vehicleController.VehicleRigidbody.linearVelocity.normalized), 
                cameraRotationSpeed * Time.deltaTime
            );
        }
    }
}
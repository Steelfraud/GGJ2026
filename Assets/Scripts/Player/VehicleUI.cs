using Sampla.Player;
using UnityEngine;

public class VehicleUI : MonoBehaviour
{
    [SerializeField] private VehicleController vehicleController;

    void OnGUI()
    {
        Rect layoutRect = new Rect(50, 50, 200, 100);
        GUI.Label(layoutRect, new GUIContent("Speed: " + MSToKMH(vehicleController.VehicleRigidbody.linearVelocity.magnitude).ToString("F1") + " km/h"));
        layoutRect.y += 15;
        GUI.Label(layoutRect, new GUIContent("RPM: " + vehicleController.GetAverageFrontWheelsRPM().ToString("F0") + " rpm"));
    }

    float MSToKMH(float metersPerSecond)
    {
        return metersPerSecond * 3.6f;
    }
}

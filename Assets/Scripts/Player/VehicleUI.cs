using TMPro;
using UnityEngine;

namespace Sampla.Player
{
    public class VehicleUI : MonoBehaviour
    {
        [SerializeField] private VehicleController vehicleController;
        [SerializeField] private TextMeshPro turboFollowersText;
        [SerializeField, Range(0, 9999999)] private int maxTurboFollowers; 

        [SerializeField] private TextMeshPro timeLimitText;

        [Space]
        [SerializeField] private bool showDebugGUI = true;

        void OnGUI()
        {
            if (!showDebugGUI)
                return;

            Rect layoutRect = new Rect(50, 50, 200, 100);
            GUI.Label(layoutRect, new GUIContent("Speed: " + vehicleController.CurrentSpeedKMH.ToString("F1") + " km/h"));
            layoutRect.y += 15;
            GUI.Label(layoutRect, new GUIContent("RPM: " + vehicleController.GetAverageFrontWheelsRPM().ToString("F0") + " rpm"));
            layoutRect.y += 15;
            GUI.Label(layoutRect, new GUIContent("Turbo Left: " + vehicleController.CurrentTurboTimeLeft.ToString("F1") + " s"));
        }

        void Update()
        {
            if (turboFollowersText != null)
            {
                turboFollowersText.text = Mathf.FloorToInt(vehicleController.NormalizedTurboLeft * maxTurboFollowers).ToString();
            }
            if (timeLimitText != null && GameManager.Instance != null)
            {
                timeLimitText.text = GameManager.Instance.CurrentTimeLeft.ToString();
            }
        }
    }
}
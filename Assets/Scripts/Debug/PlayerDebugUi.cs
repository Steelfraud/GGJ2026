using UnityEngine;
using System.Collections;
using System.Text;

public class PlayerDebugUi : MonoBehaviour
{
    [SerializeField] Rigidbody PlayerRigidbody;

    void OnGUI()
    {
        var screenHeight = Screen.height;
        var screenWidth = Screen.width;
        var boxW = 100;
        var boxH = 100;
        var offsetX = 50;
        var offsetY = 50;
        var sb = new StringBuilder();
        GUI.Box(new Rect(screenWidth - offsetX - boxW, screenHeight - offsetY - boxH, boxW, boxH), new GUIContent(sb.ToString()));
    }
}

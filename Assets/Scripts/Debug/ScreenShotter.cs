using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotter : MonoBehaviour
{

    public KeyCode ScreenshotKey = KeyCode.Print;

    private int screenshotsTaken = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(ScreenshotKey))
        {
            ScreenCapture.CaptureScreenshot("Screenshot_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + this.screenshotsTaken++ + ".png");
            Debug.Log("A screenshot was taken!");
        }
    }

}

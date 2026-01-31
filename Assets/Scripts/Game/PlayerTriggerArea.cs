using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerArea : MonoBehaviour
{
    public UnityEvent PlayerEnteredArea;
    public UnityEvent PlayerExitedArea;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        //Debug.Log("Player in area?");
        PlayerEnteredArea?.Invoke();
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.tag != "Player")
        {
            return;
        }

        //Debug.Log("Player left area?");
        PlayerExitedArea?.Invoke();
    }
    
    public void SetPlayerCameraMask(int setTo)
    {
        if (CameraHandler.Instance == null)
        {
            return;
        }

        //Debug.Log("Setting camera mask to: " + setTo);

        if (Enum.IsDefined(typeof(CameraHandler.CameraState), setTo) == false)
        {
            Debug.LogError("Tried to set bad enum value?? Value was: " + setTo);
        }
        else
        {
            CameraHandler.Instance.SetNewCameraState((CameraHandler.CameraState)setTo);
        }
    }

}
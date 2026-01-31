using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTriggerArea : MonoBehaviour
{
    public UnityEvent PlayerEnteredArea;
    public UnityEvent PlayerExitedArea;

    public bool SetPlayerMask = false;
    public CameraHandler.CameraState StateToSet = CameraHandler.CameraState.Nothing;

    private bool playerInArea = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        PlayerEntered();
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.tag != "Player")
        {
            return;
        }

        PlayerLeft();
    }

    protected virtual void PlayerEntered()
    {
        //Debug.Log("Player in area?");
        PlayerEnteredArea?.Invoke();
        playerInArea = true;

        if (SetPlayerMask)
        {
            SetMyCameraState();
        }
    }

    protected virtual void PlayerLeft()
    {
        //Debug.Log("Player left area?");
        PlayerExitedArea?.Invoke();
        playerInArea = false;
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

    protected void SetMyCameraState()
    {
        if (CameraHandler.Instance != null)
        {
            CameraHandler.Instance.SetNewCameraState(StateToSet);
        }
    }

}
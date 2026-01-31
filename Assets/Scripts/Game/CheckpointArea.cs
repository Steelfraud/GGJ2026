using UnityEngine;

public class CheckpointArea : PlayerTriggerArea
{

    public Transform ResetPoint;

    protected override void PlayerEntered()
    {
        base.PlayerEntered();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetPlayerCheckpoint(this);
        }
    }

    public void SetPlayerToCheckpoint()
    {
        if (GameManager.Instance != null) 
        {
            GameManager.Instance.PlayerVehicle.transform.position = ResetPoint.position;
            GameManager.Instance.PlayerVehicle.transform.rotation = ResetPoint.rotation;
        }

        if (SetPlayerMask)
        {
            SetMyCameraState();
        }
    }

}
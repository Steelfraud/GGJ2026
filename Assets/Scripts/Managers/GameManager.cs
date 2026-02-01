using Sampla.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public VehicleController PlayerVehicle;
    public CheckpointArea StartingCheckPoint;

    private CheckpointArea currentCheckPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (CreateSingleton(this, SetDontDestroy))
        {

        }
    }

    private void Start()
    {
        if (this.StartingCheckPoint != null)
        {
            SetPlayerCheckpoint(this.StartingCheckPoint);
        }
    }

    private void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().f5Key.wasPressedThisFrame)
        {
            ResetPlayerToLastCheckpoint();
        }
    }

    public void SetPlayerCheckpoint(CheckpointArea newCheckpoint)
    {
        currentCheckPoint = newCheckpoint;
        Debug.Log("set new player checkpoint?");
    }

    public void ResetPlayerToLastCheckpoint()
    {
        if (currentCheckPoint == null) 
        {
            Debug.LogError("Player doesnt have checkpoint?!?!");
            return;
        }

        if (PlayerVehicle == null)
        {
            Debug.LogError("Player not set?!?!");
            return;
        }

        StartCoroutine(ResetPlayer());
    }

    private IEnumerator ResetPlayer()
    {
        PlayerVehicle.ResetVehicle();
        yield return null;
        currentCheckPoint.SetPlayerToCheckpoint();
        yield return null;
        PlayerVehicle.RestartVehicle();
        Debug.Log("Player resetted?");
    }

}
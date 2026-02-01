using Sampla.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
    public VehicleController PlayerVehicle;
    public CheckpointArea StartingCheckPoint;

    [Space]
    [SerializeField, Min(0)] private float timeLimit = 90; public float TimeLimit { get { return timeLimit; } }
    [SerializeField] private AnimationCurve timeReductionMultiplierAtSpeed = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(100f, 0.2f));

    private float currentTimeLeft; public float CurrentTimeLeft { get { return currentTimeLeft; } }
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

        StartCoroutine(GameTimerRoutine());
    }

    private void Update()
    {
        if (InputSystem.GetDevice<Keyboard>().f5Key.wasPressedThisFrame)
        {
            ResetPlayerToLastCheckpoint();
        }
    }

    IEnumerator GameTimerRoutine()
    {
        float currentTimeLeft = timeLimit;

        while (currentTimeLeft > 0)
        {
            float reductionMultiplier = PlayerVehicle != null ? timeReductionMultiplierAtSpeed.Evaluate(PlayerVehicle.CurrentSpeedKMH) : 1f;

            currentTimeLeft -= Time.deltaTime * reductionMultiplier;
            yield return null;
        }

        Debug.Log("Timer run out!");
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
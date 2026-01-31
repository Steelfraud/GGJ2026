using Sampla.Player;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public VehicleController PlayerVehicle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (CreateSingleton(this, SetDontDestroy))
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
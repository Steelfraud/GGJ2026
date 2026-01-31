using Sampla.Player;
using UnityEngine;

public class CarAudioHandler : MonoBehaviour
{
    public VehicleController MyVehicle;
    public AudioSource WindSoundSource;
    public AnimationCurve WindVolumeFromSpeed;
    public AudioSource CarSoundSource;
    public AnimationCurve CarVolumeFromSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MyVehicle == null)
        {
            return;
        }

        float speedLerp = MyVehicle.CurrentSpeedLerp;
        WindSoundSource.volume = WindVolumeFromSpeed.Evaluate(speedLerp);

        CarSoundSource.volume = CarVolumeFromSpeed.Evaluate(speedLerp);
    }

}
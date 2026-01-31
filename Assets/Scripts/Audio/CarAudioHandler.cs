using Sampla.Player;
using UnityEngine;

public class CarAudioHandler : MonoBehaviour
{
    public VehicleController MyVehicle;
    public AudioSource WindSoundSource;
    public AnimationCurve WindVolumeFromSpeed;
    public AudioSource CarSoundSource;
    public AnimationCurve CarVolumeFromSpeed;

    public AudioSource CrashSource;
    public float CrashPitchMaxOffset;
    public float MinTimeBetweenCrashSounds = 0.5f;

    private float lastCrashSound = 0f;

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

    public void PlayCrashingSound()
    {
        if (Time.timeSinceLevelLoad - lastCrashSound < MinTimeBetweenCrashSounds)
        {
            return;
        }

        if (CrashSource.isPlaying)
        {
            return;
        }

        Debug.Log("CRASH");
        float pitch = 1f;

        if (CrashPitchMaxOffset > 0f)
        {
            pitch += Random.Range(-CrashPitchMaxOffset, CrashPitchMaxOffset);
        }

        CrashSource.pitch = pitch;
        CrashSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlayCrashingSound();
    }

}
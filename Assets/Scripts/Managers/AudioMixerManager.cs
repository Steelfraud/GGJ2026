using System;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
///     Used to change volume of the different audio groups.
/// </summary>
public class AudioMixerManager : Singleton<AudioMixerManager>
{
    public AudioMixer usedMixer;
    public string MasterVolumeName = "MasterVolume";
    public string EffectsVolumeName = "EffectsVolume";
    public string MusicVolumeName = "MusicVolume";

    public AnimationCurve volumeCurve;

    private float currentMasterVolumeLerp = 1f;
    public float MasterVolume
    {
        get
        {
            return this.currentMasterVolumeLerp;
        }
        set
        {
            this.currentMasterVolumeLerp = Mathf.Clamp01(value);
            float valueToSet = this.volumeCurve.Evaluate(this.currentMasterVolumeLerp);

            AudioMixer audioMixer = this.usedMixer;

            if (audioMixer != null && this.volumeCurve != null)
            {
                audioMixer.SetFloat(this.MasterVolumeName, valueToSet);
            }
        }
    }

    private float currentEffectsVolumeLerp = 1f;
    public float EffectsVolume
    {
        get
        {
            return this.currentEffectsVolumeLerp;
        }
        set
        {
            this.currentEffectsVolumeLerp = Mathf.Clamp01(value);
            float valueToSet = this.volumeCurve.Evaluate(this.currentEffectsVolumeLerp);

            AudioMixer audioMixer = this.usedMixer;

            if (audioMixer != null && this.volumeCurve != null)
            {
                audioMixer.SetFloat(this.EffectsVolumeName, valueToSet);
            }
        }
    }

    private float currentMusicVolumeLerp = 1f;
    public float MusicVolume
    {
        get
        {
            return this.currentMusicVolumeLerp;
        }
        set
        {
            this.currentMusicVolumeLerp = Mathf.Clamp01(value);
            float valueToSet = this.volumeCurve.Evaluate(this.currentMusicVolumeLerp);

            AudioMixer audioMixer = this.usedMixer;

            if (audioMixer != null && this.volumeCurve != null)
            {
                audioMixer.SetFloat(this.MusicVolumeName, valueToSet);
            }
        }
    }

    private void Start()
    {
        if (CreateSingleton(this, this.SetDontDestroy))
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        //MasterVolume = DataManager.Instance.PlayerSettings.CurrentMasterVolume;
        //MusicVolume = DataManager.Instance.PlayerSettings.CurrentMusicVolume;
        //EffectsVolume = DataManager.Instance.PlayerSettings.CurrentEffectsVolume;
    }

}
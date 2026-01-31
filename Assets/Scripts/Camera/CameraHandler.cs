using RotaryHeart.Lib.SerializableDictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler : Singleton<CameraHandler>
{
    public enum CameraState
    {
        Nothing,
        VerticalLeft,
        VerticalRight,
        HorizontalBottom,
        HorizontalTop,
        RotationLoop,
        DVDLoop,
    }

    [Serializable]
    public class CameraSettingsDictionary : SerializableDictionaryBase<CameraState, CameraStateSettings> { };

    [Serializable]
    public class CameraStateSettings
    {
        public string AnimationBool;
    }

    public Animator CameraDimensionAnimator;
    public CameraSettingsDictionary CameraSettingsDict;

    public CameraState CurrentState = CameraState.Nothing;

    private CameraStateSettings currentSettings => CameraSettingsDict[lastKnownState];
    private CameraState lastKnownState = CameraState.Nothing;

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

        
        if (InputSystem.GetDevice<Keyboard>().f10Key.wasPressedThisFrame)
        {
            CameraState stateToSet = CameraState.Nothing;
            List<CameraState> statesInDict = CameraSettingsDict.Keys.ToList();

            if (lastKnownState == CameraState.Nothing)
            {
                stateToSet = statesInDict[0];
            }
            else
            {
                int stateIndex = statesInDict.IndexOf(lastKnownState);

                if (stateIndex >= 0)
                {
                    stateIndex++;

                    if (stateIndex < statesInDict.Count)
                    {
                        stateToSet |= statesInDict[stateIndex];
                    }
                }
                else
                {
                    stateToSet = statesInDict[0];
                }
            }

            CurrentState = stateToSet;
        }
        else if (InputSystem.GetDevice<Keyboard>().f11Key.wasPressedThisFrame)
        {
            CurrentState = CameraState.Nothing;
        }

        if (lastKnownState != CurrentState)
        {
            SetNewCameraState(CurrentState);

            if (lastKnownState != CurrentState)
            {
                lastKnownState = CurrentState;
            }
        }
    }

    public void SetNewCameraState(CameraState newCameraState)
    {
        if (lastKnownState == CurrentState)
        {
            return;
        }

        if (newCameraState != CameraState.Nothing && CameraSettingsDict.ContainsKey(newCameraState) == false)
        {
            Debug.LogError("Trying to set an camera state with no settings?? Pls fix, state is: " + newCameraState);
            return;
        }

        if (lastKnownState != CameraState.Nothing) 
        {
            CameraDimensionAnimator.SetBool(currentSettings.AnimationBool, false);
        }

        CurrentState = newCameraState;
        lastKnownState = CurrentState;

        if (CurrentState != CameraState.Nothing)
        {
            CameraDimensionAnimator.SetBool(currentSettings.AnimationBool, true);
        }
    }

}
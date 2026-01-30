using RotaryHeart.Lib.SerializableDictionary;
using System;
using UnityEngine;

[Serializable]
public class TextToSpriteDictionary : SerializableDictionaryBase<string, Sprite> { }

[Serializable]
public class StringToIntDictionary : SerializableDictionaryBase<string, int> { }

[Serializable]
public class ThemeToPrefabDict : SerializableDictionaryBase<LevelPieceVisualTheme, GameObject> { }

[Serializable]
public class ThemeToPoolableDict : SerializableDictionaryBase<LevelPieceVisualTheme, PooledPrefabData> { }

[Serializable]
public class PoolableDictionary : SerializableDictionaryBase<string, PoolableSettings> { };

[System.Serializable]
public class FloatPair
{

    public FloatPair()
    {

    }

    public FloatPair(float first, float second)
    {
        this.FirstValue = first;
        this.SecondaryValue = second;
    }

    public float FirstValue;
    public float SecondaryValue;

}

[System.Serializable]
public class IntPair
{

    public IntPair()
    {

    }

    public IntPair(int first, int second)
    {
        this.FirstValue = first;
        this.SecondaryValue = second;
    }

    public IntPair(IntPair copy)
    {
        this.FirstValue = copy.FirstValue;
        this.SecondaryValue = copy.SecondaryValue;
    }

    public override bool Equals(object obj)
    {
        if (obj is IntPair pair)
        {
            return pair.FirstValue == this.FirstValue && pair.SecondaryValue == this.SecondaryValue;
        }

        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public static bool operator ==(IntPair left, IntPair right)
    {
        return left.FirstValue == right.FirstValue && left.SecondaryValue == right.SecondaryValue;
    }

    public static bool operator !=(IntPair left, IntPair right)
    {
        return !(left == right);
    }

    public static IntPair operator +(IntPair left, IntPair right)
    {
        return new IntPair(left.FirstValue + right.FirstValue, left.SecondaryValue + right.SecondaryValue);
    }

    public IntPair ShallowClone()
    {
        return this.MemberwiseClone() as IntPair;
    }

    public int FirstValue;
    public int SecondaryValue;

}

[System.Serializable]
public class AllowedInputs
{

    public bool AllInputsAllowed = true;
    public InputType AllowedInput = InputType.All;

}

public enum GameLockableFeature
{

    None,
    BotControls,
    BallMode,

}

[Serializable]
public enum LevelPieceVisualTheme
{

    Basic = 0,
    Colorful = 1,

}

[Serializable]
public class PoolableSettings
{

    public string Identifier;
    public PooledPrefabData PoolableToCreate;
    public PoolObjectSettings SettingsToSet;
    public bool UniquePoolable = false;

}

public enum DataTrigger
{
    GameStart = 101,
    GameEnd = 102,
    AppSwitch = 103,
    AppTerminate = 104,

    ControlTaskStart = 201,
    ControlTaskSuccess = 202,
    ControlTaskFail = 203,
    ControlTaskEnd = 204,
    ControlTaskRestart = 205,
    ControlSwitchSuccess = 206,
    ControlSwitchFail = 207,

    FocusTaskStart = 301,
    FocusTaskSuccess = 302,
    FocusTaskFail = 303,
    FocusTaskEnd = 304,
    FocusTaskRestart = 305,
    FocusNoGoSuccess = 306,
    FocusNoGoFail = 307,

    UpdateTaskStart = 401,
    UpdateTaskSuccessful = 402,
    UpdateTaskFail = 403,
    UpdateTaskEnd = 404,
    UpdateTaskRestart = 405,

    InferenceTaskStart = 501,
    InferenceTaskCompleted = 502,
    InferenceTaskFailed = 503,
    InferenceTaskEnd = 504, 
    InferenceTaskRestart = 505,

    PlanningTaskStart = 601,
    PlanningTaskCompleted = 602,
    PlanningTaskFailed  = 603,
    PlanningTaskEnd = 604,
    PlanningTaskRestart = 605,
    
    PlanningStartingConfiguration = 650,
    PlanningSwitchView = 651,
    PlanningViewLock = 652,
    PlanningStartedMovingElement = 653,
    PlanningMovingElementFinished = 654,

}

public enum DataCategory
{

    General = 1,
    ControlTask = 2,
    FocusTask = 3,
    UpdateTask = 4,
    InferenceTask = 5, //also known as reasoning task
    PlanningTask = 6,

}

[Flags]
public enum InputType
{

    BotInputs = 1 << 1,
    BallInputs = 1 << 2,
    UIInputs = 1 << 3,
    WalkerInputs = 1 << 4,


    MovementInputs = BallInputs | WalkerInputs,
    All = ~0

}

[System.Serializable]
public class PlayerGameSettings
{

    public float CurrentMasterVolume
    {
        get => this.MasterVolume;
        set
        {
            this.MasterVolume = value;
            
            if (AudioMixerManager.Instance != null)
            {
                AudioMixerManager.Instance.MasterVolume = this.MasterVolume;
            }
        }
    }

    public float CurrentMusicVolume
    {
        get => this.MusicVolume;
        set
        {
            this.MusicVolume = value;

            if (AudioMixerManager.Instance != null)
            {
                AudioMixerManager.Instance.MusicVolume = this.MusicVolume;
            }
        }
    }

    public float CurrentEffectsVolume
    {
        get => this.SoundEffectsVolume;
        set
        {
            this.SoundEffectsVolume = value;

            if (AudioMixerManager.Instance != null)
            {
                AudioMixerManager.Instance.EffectsVolume = this.SoundEffectsVolume;
            }
        }
    }


    private float MasterVolume = 1f;
    private float MusicVolume = 1f;
    private float SoundEffectsVolume = 1f;

    public int CurrentInputScheme = -1;
    public int QualitySettings = 2;

}
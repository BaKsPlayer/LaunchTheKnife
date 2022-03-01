using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VibrationType { Error, Success, Light, Medium, Heavy, Soft }

public class VibrationManager : MonoBehaviour
{

    IVibrator _vibrator;

    public static VibrationManager instance;

    private void Awake()
    {
#if UNITY_IOS && !UNITY_EDITOR
       _vibrator = gameObject.AddComponent<IOSVibrator>();
#elif UNITY_ANDROID && !UNITY_EDITOR
        _vibrator = gameObject.AddComponent<AndroidVibrator>();
#endif

        if (instance != null)
            if (instance != this)
                Destroy(this);

        DontDestroyOnLoad(this);
        instance = this;
    }

    public void Vibrate(VibrationType type)
    {
        if (PlayerPrefsSafe.GetInt("Vibration") == 1)
        {
            _vibrator?.Vibrate(type);
        }

    }

}

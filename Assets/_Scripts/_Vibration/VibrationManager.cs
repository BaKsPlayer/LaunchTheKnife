using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VibrationType { Error, Success, Light, Medium, Heavy, Soft }

public class VibrationManager : MonoBehaviour
{

    IVibrator _vibrator;

    private void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
       _vibrator = gameObject.AddComponent<IOSVibrator>();
#elif UNITY_ANDROID
        _vibrator = gameObject.AddComponent<AndroidVibrator>();    
#endif
    }

    public void Vibrate(VibrationType type)
    {
        if (PlayerPrefsSafe.GetInt("Vibration") == 1)
        {
            _vibrator?.Vibrate(type);
        }

    }

}

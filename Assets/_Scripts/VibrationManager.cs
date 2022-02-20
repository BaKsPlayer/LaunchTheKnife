using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public enum VibraType { Error, Success, Light, Medium, Heavy, Soft}
    


    public void Vibrate(VibraType type)
    {
        if (PlayerPrefsSafe.GetInt("Vibration") == 1)
        {
#if UNITY_IOS && !UNITY_EDITOR

            if (type == VibraType.Error)
                HapticEngine.NotificationFeedbackError();

            if (type == VibraType.Success)
                HapticEngine.NotificationFeedbackSuccess();

            if (type == VibraType.Light)
                HapticEngine.ImpactFeedbackLight();

            if (type == VibraType.Medium)
                HapticEngine.ImpactFeedbackMedium();

            if (type == VibraType.Heavy)
                HapticEngine.ImpactFeedbackHeavy();

            if (type == VibraType.Soft)
                HapticEngine.ImpactFeedbackSoft();

#elif UNITY_ANDROID && !UNITY_EDITOR

            if (type == VibraType.Error)
                AndroidFeedbackError();

            if (type == VibraType.Success)
                AndroidFeedbackSuccess();

            if (type == VibraType.Light)
                AndroidFeedbackLight();

            if (type == VibraType.Medium)
                AndroidFeedbackMedium();

            if (type == VibraType.Heavy)
                AndroidFeedbackHeavy();

            if (type == VibraType.Soft)
                AndroidFeedbackSoft();

#elif UNITY_EDITOR
            print("Vibration - " + type);
#endif
        }

    }

    public void AndroidFeedbackError()
    {
        StartCoroutine(ErrorCoroutine());
    }

    IEnumerator ErrorCoroutine()
    {
        int k = 0;

        float timer = 0;

        while (k != 3)
        {
            if (timer < 0.07f)
            timer += Time.deltaTime;
            else
            {
                timer = 0;
                k++;

                Vibration.Vibrate(30);

            }

            yield return null;
        }

    }

    public void AndroidFeedbackSuccess()
    {
        StartCoroutine(SuccessCoroutine());
    }

    IEnumerator SuccessCoroutine()
    {
        int k = 0;

        float timer = 0;

        while (k != 2)
        {
            if (timer < 0.2f)
                timer += Time.deltaTime;
            else
            {
                timer = 0;
                k++;

                if (k == 1)
                    Vibration.Vibrate(35);
                else
                    Vibration.Vibrate(40);

            }

            yield return null;
        }

    }

    public void AndroidFeedbackLight()
    {
        Vibration.Vibrate(20);
    }

    public void AndroidFeedbackMedium()
    {
        Vibration.Vibrate(30);
    }

    public void AndroidFeedbackHeavy()
    {
        Vibration.Vibrate(40);
    }

    public void AndroidFeedbackSoft()
    {
        Vibration.Vibrate(20);
    }
}

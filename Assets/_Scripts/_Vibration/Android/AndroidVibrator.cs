using System.Collections;
using UnityEngine;

public class AndroidVibrator : MonoBehaviour, IVibrator
{
    public void Vibrate(VibrationType type)
    {
        switch (type)
        {
            case VibrationType.Error:
                AndroidFeedbackError();
                break;

            case VibrationType.Success:
                AndroidFeedbackSuccess();
                break;

            case VibrationType.Light:
                AndroidFeedbackLight();
                break;

            case VibrationType.Medium:
                AndroidFeedbackMedium();
                break;

            case VibrationType.Heavy:
                AndroidFeedbackHeavy();
                break;
        }
    }


    public void AndroidFeedbackError() => StartCoroutine(ErrorCoroutine());

    public void AndroidFeedbackSuccess() => StartCoroutine(SuccessCoroutine());

    public static void AndroidFeedbackLight() => AndroidVibrationEngine.Vibrate(20);

    public static void AndroidFeedbackMedium() => AndroidVibrationEngine.Vibrate(30);

    public static void AndroidFeedbackHeavy() => AndroidVibrationEngine.Vibrate(40);

    private IEnumerator SuccessCoroutine()
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
                    AndroidVibrationEngine.Vibrate(35);
                else
                    AndroidVibrationEngine.Vibrate(40);
            }

            yield return null;
        }
    }

    private IEnumerator ErrorCoroutine()
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

                AndroidVibrationEngine.Vibrate(30);
            }

            yield return null;
        }

    }

}

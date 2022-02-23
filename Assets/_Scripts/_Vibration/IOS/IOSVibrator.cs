using UnityEngine;

public class IOSVibrator : MonoBehaviour, IVibrator
{
    public void Vibrate(VibrationType type)
    {
        switch (type)
        {
            case VibrationType.Error:
                HapticEngine.NotificationFeedbackError();
                break;

            case VibrationType.Success:
                HapticEngine.NotificationFeedbackSuccess();
                break;

            case VibrationType.Light:
                HapticEngine.ImpactFeedbackLight();
                break;

            case VibrationType.Medium:
                HapticEngine.ImpactFeedbackMedium();
                break;

            case VibrationType.Heavy:
                HapticEngine.ImpactFeedbackHeavy();
                break;
        }
    }
}

using UnityEngine;

public class RateUs : MonoBehaviour
{
    public void OpenStorePage()
    {
#if UNITY_ANDROID || UNITY_EDITOR
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.EvilByte.LaunchTheKnife");
#endif
    }
}

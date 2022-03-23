using UnityEngine;

public class ScreenScaleFitter : MonoBehaviour
{
    private void Start()
    {
        float currentRatio = (float)Screen.height / Screen.width;

        float referenceRatio = 2436f / 1125f;

        float scale = currentRatio / referenceRatio;

        GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
    }
}

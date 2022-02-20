using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSizeFitter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float x = (float)Screen.height / Screen.width;

        float reference = 2436f / 1125f;

        float scale = x / reference;


        GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

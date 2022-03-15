using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapHandler : MonoBehaviour, IPointerDownHandler
{
    public GameKnife gameKnife;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameKnife.Launch();
    }
}

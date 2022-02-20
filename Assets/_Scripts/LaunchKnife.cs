using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LaunchKnife : MonoBehaviour, IPointerDownHandler
{
    public PlayerController playerController;

    public void OnPointerDown(PointerEventData eventData)
    {
        playerController.LaunchKnife();
    }



}

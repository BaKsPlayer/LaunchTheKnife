using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameKnife gameKnife;

    public Action OnClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameKnife.Launch();

        OnClick?.Invoke();
    }
}

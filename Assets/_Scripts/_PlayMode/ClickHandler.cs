using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickHandler : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameKnife gameKnife;

    public UnityAction OnClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        gameKnife.Launch();

        OnClick?.Invoke();
    }
}

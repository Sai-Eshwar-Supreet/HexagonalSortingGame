using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonPressHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public event Action<bool> OnPressStateChange;
    public void OnPointerDown(PointerEventData eventData)
    {
        var selectable = eventData.pointerCurrentRaycast.gameObject.GetComponent<Selectable>();
        if (selectable == null || !selectable.interactable) return;
        OnPressStateChange?.Invoke(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var selectable = eventData.pointerCurrentRaycast.gameObject.GetComponent<Selectable>();
        if (selectable == null || !selectable.interactable) return;
        OnPressStateChange?.Invoke(false);
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIClickSoundHandler : SoundHandler, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        var selectable = eventData.pointerPress.GetComponent<Selectable>();
        if (selectable == null) return;
        if (selectable.interactable) PlaySound();
    }
}

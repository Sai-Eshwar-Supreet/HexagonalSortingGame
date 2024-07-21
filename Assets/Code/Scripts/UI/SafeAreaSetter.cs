using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaSetter : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;

    private Rect currentSafeArea = new();
    private ScreenOrientation currentOrientation = ScreenOrientation.AutoRotation;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        currentOrientation = Screen.orientation;
        currentSafeArea = Screen.safeArea;

        ApplySafeArea();
    }

    private void Update()
    {
        if (currentSafeArea != Screen.safeArea || currentOrientation != Screen.orientation) ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (rectTransform == null) return;

        Rect safeArea = Screen.safeArea;

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;

        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        currentOrientation = Screen.orientation;
        currentSafeArea = Screen.safeArea;
    }
}

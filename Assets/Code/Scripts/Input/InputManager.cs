using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-1)]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance => instance;
    private static InputManager instance;

    public event Action OnPrimaryFingerDown;
    public event Action OnPrimaryFingerMove;
    public event Action OnPrimaryFingerUp;

    private Finger primaryFinger;
    private int uiLayer;

    public Finger PrimaryFinger => primaryFinger;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;

        uiLayer = LayerMask.NameToLayer("UI");
    }
    private void OnEnable()
    {
        ETouch.EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += FingerDownHandler;
        ETouch.Touch.onFingerMove += FingerMoveHandler;
        ETouch.Touch.onFingerUp += FingerUpHandler;
    }
    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= FingerDownHandler;
        ETouch.Touch.onFingerMove -= FingerMoveHandler;
        ETouch.Touch.onFingerUp -= FingerUpHandler;
        ETouch.EnhancedTouchSupport.Disable();
    }
    private void FingerDownHandler(Finger finger)
    {
        if (IsOnUI(finger.screenPosition)) return;
        if (primaryFinger == null)
        {
            primaryFinger = finger;
            OnPrimaryFingerDown?.Invoke();
        }

    }
    private void FingerMoveHandler(Finger finger)
    {
        if (finger == primaryFinger)
        {
            OnPrimaryFingerMove?.Invoke();
        }

    }
    private void FingerUpHandler(Finger finger)
    {
        if (finger == primaryFinger)
        {
            primaryFinger = null;
            OnPrimaryFingerUp?.Invoke();
        }
    }
    private bool IsOnUI(Vector2 screenPos)
    {
        PointerEventData pointerEventData = new(EventSystem.current)
        {
            position = screenPos
        };
        List<RaycastResult> results = new();

        EventSystem.current.RaycastAll(pointerEventData, results);

        results = results.Where((result) => result.gameObject.layer == uiLayer).ToList();

        return results.Count > 0;
    }
}

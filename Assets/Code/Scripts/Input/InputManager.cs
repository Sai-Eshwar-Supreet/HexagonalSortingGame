using System;
using UnityEngine;
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
    public Finger PrimaryFinger => primaryFinger;
    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
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
}

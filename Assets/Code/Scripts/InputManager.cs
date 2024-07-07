using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
    public static InputManager Instance => instance;
    private static InputManager instance;

    public event Action OnPrimaryFingerDown;
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
        ETouch.Touch.onFingerUp += FingerUpHandler;
    }
    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= FingerDownHandler;
        ETouch.Touch.onFingerUp -= FingerUpHandler;
        ETouch.EnhancedTouchSupport.Disable();
    }
    private void FingerDownHandler(Finger finger)
    {
        if (primaryFinger == null)
        {
            primaryFinger = finger;
            OnPrimaryFingerUp?.Invoke();
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

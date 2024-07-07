using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float movementTime;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float rotateTimeout = 1;
    [SerializeField] private float minSwipeThreshold = 20;
    [SerializeField] private Vector3 zoomAmount;
    [SerializeField] private Vector3 minZoomValue = new(0, 5, -50);
    [SerializeField] private Vector3 maxZoomValue = new(0, 50, -5);
    [SerializeField] private Transform camTransform;
    private Finger primaryFinger;
    private Vector2 primaryStartPos;
    private Finger secondaryFinger;
    private Vector2 secondaryStartPos;

    private float previousDistance;
    private Vector3 targetZoom;

    private void OnEnable()
    {
        ETouch.EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += FingerDownHandler;
        ETouch.Touch.onFingerMove += FingerMoveHandler;
        ETouch.Touch.onFingerUp += FingerUpHandler;
        targetZoom = camTransform.localPosition;
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
            primaryStartPos = finger.screenPosition;
        }
        else if (secondaryFinger == null)
        {
            secondaryFinger = finger;
            secondaryStartPos = finger.screenPosition;
            previousDistance = Vector2.Distance(primaryStartPos, secondaryStartPos);
        }
    }

    private void FingerMoveHandler(Finger finger)
    {
        if (primaryFinger != null)
        {
            if (secondaryFinger != null)
            {
                //pinch
                float currentDistance = Vector2.Distance(primaryFinger.screenPosition,
                    secondaryFinger.screenPosition);
                targetZoom += (currentDistance > previousDistance ? 1 : -1) * zoomAmount;
                targetZoom = Vector3.Max(minZoomValue, Vector3.Min(maxZoomValue, targetZoom));
                previousDistance = currentDistance;
            }
            else
            {
                //rotate
                ETouch.Touch currentTouch = finger.currentTouch;
                double time = currentTouch.time - currentTouch.startTime;
                float xDelta = (finger.screenPosition - primaryStartPos).x;
                if (time <= rotateTimeout && Mathf.Abs(xDelta) > minSwipeThreshold)
                {
                    transform.Rotate(new Vector3(0, xDelta, 0) * rotateSpeed, Space.World);
                }
            }
        }
    }
    private void FingerUpHandler(Finger finger)
    {
        if (finger == primaryFinger)
        {
            primaryFinger = secondaryFinger;
            secondaryFinger = null;
        }
        else if (finger == secondaryFinger)
        {
            secondaryFinger = null;
        }
    }

    private void Update()
    {
        if (!Vector3.Equals(targetZoom, camTransform.localPosition))
        {
            camTransform.localPosition = Vector3.Slerp(camTransform.localPosition,
                targetZoom, Time.deltaTime * movementTime);
        }
    }

}

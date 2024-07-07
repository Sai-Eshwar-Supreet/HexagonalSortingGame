using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class AttachToGrid : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private bool isPlaced;
    private Vector3 targetPos;

    private Vector3 initPos;

    private readonly Plane plane = new(Vector3.up, Vector3.zero);

    private void Awake()
    {
        targetPos = initPos = transform.position;
    }
    private void OnEnable()
    {
        InputManager.Instance.OnPrimaryFingerUp += FingerUpHandler;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPrimaryFingerUp -= FingerUpHandler;
    }

    private void FingerUpHandler()
    {
        if (isPlaced) return;
        transform.position = targetPos;
    }
    public void OnDrag()
    {
        if (isPlaced) return;
        Finger finger = InputManager.Instance.PrimaryFinger;
        if (finger == null) return;
        Vector3 touchPos = finger.screenPosition;
        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, mask))
        {
            targetPos = hitInfo.transform.position;
        }
        else
        {
            targetPos = initPos;
        }

        if(plane.Raycast(ray, out float enter))
        {
            Vector3 point = ray.GetPoint(enter);
            transform.position = point + Vector3.up * 2;
        }
    }

    public void OnPointerUp()
    {
        if (isPlaced) return;
        if(targetPos != initPos) isPlaced = true;
        transform.DOMove(targetPos, 0.25f);
    }
}

using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class StackMovementController : MonoBehaviour
{
    [SerializeField] private LayerMask hexElementMask;
    [SerializeField] private LayerMask hexCellMask;
    [SerializeField] private LayerMask groundMask;

    public static Action<HexCell> OnPlacedStack;

    private HexStack currentStack;
    private Vector3 currentStackInitPos;

    private HexCell currentGridCell;
    private Vector3 targetPos;

    private readonly Plane plane = new(Vector3.up, Vector3.zero);

    private void OnEnable()
    {
        InputManager.Instance.OnPrimaryFingerDown += FingerDownHandler;
        InputManager.Instance.OnPrimaryFingerMove += FingerMoveHandler;
        InputManager.Instance.OnPrimaryFingerUp += FingerUpHandler;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnPrimaryFingerDown -= FingerDownHandler;
        InputManager.Instance.OnPrimaryFingerMove -= FingerMoveHandler;
        InputManager.Instance.OnPrimaryFingerUp -= FingerUpHandler;
    }
    private void FingerDownHandler()
    {
        if (GetRay(out Ray ray) && Physics.Raycast(ray, out RaycastHit hitInfo, 500, hexElementMask))
        {
            currentStack = hitInfo.transform.GetComponent<HexElement>().HexStack;
            targetPos = currentStackInitPos = currentStack.transform.position;
        }
        
    }
    private void FingerUpHandler()
    {
        if (currentStack == null) return;
        if(currentGridCell != null) currentStack.DisableInteraction();
        currentStack.transform.DOMove(targetPos, 0.05f).SetEase(Ease.InOutSine).onComplete += () =>
        {
            if (currentGridCell != null)
            {
                currentGridCell.PlaceHexStack(currentStack);
                currentGridCell.OnPointerExit();

                OnPlacedStack?.Invoke(currentGridCell);

                currentGridCell = null;
            }
            currentStack = null;
        };
        

    }
    private void FingerMoveHandler()
    {
        if (currentStack == null) return;

        if (!GetRay(out Ray ray)) return;

        bool isHit = Physics.Raycast(ray, out RaycastHit hitInfo, 500, hexCellMask);



        if (isHit)
        {
            if (currentGridCell != null) currentGridCell.OnPointerExit();
            currentGridCell = hitInfo.transform.GetComponent<HexCell>();
        }

        if (isHit && currentGridCell != null && !currentGridCell.IsOccupied)
        {
            currentGridCell.OnPointerEnter();
            targetPos = hitInfo.transform.position;
        }
        else
        {
            if (currentGridCell != null) currentGridCell.OnPointerExit();
            targetPos = currentStackInitPos;
            currentGridCell = null;
        }

        if (plane.Raycast(ray, out float enter))
        {
            Vector3 point = ray.GetPoint(enter);
            currentStack.transform.position = point + Vector3.up * 2;
        }
    }

    private bool GetRay(out Ray ray)
    {
        Finger finger = InputManager.Instance.PrimaryFinger;

        ray = new Ray();
        if (finger == null) return false;

        Vector3 touchPos = finger.screenPosition;



        ray = Camera.main.ScreenPointToRay(touchPos);
        return true;
    }
}

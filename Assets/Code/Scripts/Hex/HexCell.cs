using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    [SerializeField] private Material normalMat;
    [SerializeField] private Material hoveredMat;
    [SerializeField] private MeshRenderer meshRenderer;

    private HexStack occupantStack;
    public bool IsOccupied => occupantStack != null;
    public HexStack OccupantStack => occupantStack;

    public void OnPointerEnter()
    {
        meshRenderer.sharedMaterial = hoveredMat;
    }

    public void OnPointerExit()
    {
        meshRenderer.sharedMaterial = normalMat;
    }

    public void SetScale(float hexDiameter)
    {
        transform.localScale = Vector3.one * hexDiameter;
    }

    public void SetRotation(bool isFlatTopped)
    {
        transform.localRotation = (isFlatTopped) ?
            Quaternion.Euler(-90, 0, 90) : Quaternion.Euler(-90, 0, 0);
    }
    public void PlaceHexStack(HexStack stack)
    {
        occupantStack = stack;
        stack.transform.SetParent(transform, true);
    }
}

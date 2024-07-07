using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public class HexGridCell : MonoBehaviour
{
    [SerializeField] private Material normalMat;
    [SerializeField] private Material hoveredMat;

    public event Action<HexGridCell> OnHover;

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnPointerEnter()
    {
        meshRenderer.sharedMaterial = hoveredMat;
        OnHover?.Invoke(this);
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
}

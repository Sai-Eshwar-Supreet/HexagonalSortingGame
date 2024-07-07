using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Configuration", menuName = "Grid/Configuration")]
public class GridConfiguration : ScriptableObject
{
    [SerializeField] private int axialSize = 5;
    [SerializeField] private int rows = 5, columns = 5;
    [SerializeField] private float innerRadius = 0.5f;
    [SerializeField] private float thickness = 1f;
    [SerializeField] private float height = 1f;
    [SerializeField] private bool isFlatTopped;
    [SerializeField] private bool isAxial;

    public int AxialSize => axialSize;
    public int RowSize => rows;
    public int ColumnSize => columns;
    public float InnerRadius => innerRadius;
    public float OuterRadius => innerRadius + thickness;
    public float Height => height;
    public bool IsFlatTopped => isFlatTopped;
    public bool IsAxial => isAxial;
}

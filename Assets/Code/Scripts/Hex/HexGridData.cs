using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell
{
    [SerializeField] private Vector2Int index;
    public bool ShouldInitStacks;
    public Vector2Int Index => index;

    public GridCell(Vector2Int index, bool isInteractable = false)
    {
        this.index = index;
        ShouldInitStacks = isInteractable;
    }
}

[CreateAssetMenu(fileName = "new HexGridData", menuName = "SO/Hex/HexGridData", order = 1)]
public class HexGridData : ScriptableObject
{
    public List<GridCell> hexCells = new();
}

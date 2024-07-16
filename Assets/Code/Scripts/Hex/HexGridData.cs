using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GridCell
{
    public Vector2Int Index { get; private set; }
    public bool ShoudInitStacks;

    public GridCell(Vector2Int index, bool isInteractable = false)
    {
        this.Index = index;
        this.ShoudInitStacks = isInteractable;
    }
}

[CreateAssetMenu(fileName = "new HexGridData", menuName = "SO/Hex/HexGridData", order = 1)]
public class HexGridData : ScriptableObject
{
    public List<GridCell> hexCells = new();
}

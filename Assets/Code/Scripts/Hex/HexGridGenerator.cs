using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class HexGridGenerator : MonoBehaviour
{
    private static HexGridGenerator instance;

    [Header("References")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject hexCellPrefab;

    [Header("Grid Settings")]
    [SerializeField] private bool isAxial = true;
    [SerializeField, Min(1)] private int gridSize = 3;

    private float maxDistance;

    private readonly Dictionary<Vector2Int, HexCell> cells = new();

    private readonly List<Vector3> neighboursOffset = new();

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;
    }
    private void OnDestroy()
    {
        if(instance == this) instance = null;
    }

    private void OnEnable()
    {
        GenerateGrid();
    }
    public void GenerateGrid()
    {
        transform.Clear();
        cells.Clear();

        maxDistance = grid.CellToWorld(Vector3Int.right).magnitude * gridSize;

        SetupNeighborsOffset();

        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++) CreateCell(x, y);
        }
    }
    private void SetupNeighborsOffset()
    {
        neighboursOffset.Clear();

        neighboursOffset.Add(grid.CellToWorld(Vector3Int.up));
        neighboursOffset.Add(grid.CellToWorld(Vector3Int.down));
        neighboursOffset.Add(grid.CellToWorld(Vector3Int.left));
        neighboursOffset.Add(grid.CellToWorld(Vector3Int.right));
        neighboursOffset.Add(grid.CellToWorld(Vector3Int.up + Vector3Int.left));
        neighboursOffset.Add(grid.CellToWorld(Vector3Int.down + Vector3Int.left));

    }

    private void CreateCell(int x, int y)
    {
        Vector3 spawnPos = grid.CellToWorld(new Vector3Int(x, y, 0));
        if (isAxial && spawnPos.magnitude > maxDistance) return;

        HexCell cell = Instantiate(hexCellPrefab, spawnPos, Quaternion.identity, transform).GetComponent<HexCell>();
        cells.Add(new Vector2Int(x, y), cell);
        cell.gameObject.name = $"Cell ({x}, {y})";
    }

    public static List<HexCell> GetNeighbors_Static(HexCell cell) => instance.GetNeighbors(cell);

    public List<HexCell> GetNeighbors(HexCell cell)
    {
        List<HexCell> neighbors = new();

        foreach(var offset in neighboursOffset)
        {
            Vector3 worldPos = cell.transform.position + offset;
            Vector2Int cellPos = (Vector2Int) grid.WorldToCell(worldPos);
            if(cells.ContainsKey(cellPos)) neighbors.Add(cells[cellPos]);
        }

        return neighbors;
    }
}

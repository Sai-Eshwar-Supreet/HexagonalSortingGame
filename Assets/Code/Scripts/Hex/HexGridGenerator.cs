using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    private static HexGridGenerator instance;

    [Header("References")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject hexCellPrefab;
    [SerializeField] private TargetGroupManager targetGroupManager;
    [SerializeField] private Transform hexSpawnTransform;

    private readonly Dictionary<Vector2Int, HexCell> cells = new();

    private readonly List<Vector3> neighboursOffset = new();

    private float hexSpawnerZ;

    private void Awake()
    {
        if (instance == null) instance = this;
        else enabled = false;

        SetupNeighborsOffset();
    }
    private void OnEnable()
    {
        GenerateGrid();
    }
    private void OnDestroy()
    {
        if(instance == this) instance = null;
    }
    private void GenerateGrid()
    {
        foreach(var cell in cells.Values)
        {
            targetGroupManager.RemoveTarget(cell.transform);
        }
        
        transform.Clear();
        cells.Clear();

        hexSpawnerZ = 0;

        foreach(var gridCell in LevelManager.Instance.CurrentLevelData.GridData.hexCells)
        {
            CreateCell(gridCell.Index.x, gridCell.Index.y, gridCell.ShouldInitStacks);
        }

        Vector3 hexSpawnerPos = hexSpawnTransform.position;
        hexSpawnerPos.z = hexSpawnerZ - 2;
        hexSpawnTransform.position = hexSpawnerPos;
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

    private void CreateCell(int x, int y, bool shouldInitStacks)
    {
        Vector3 spawnPos = grid.CellToWorld(new Vector3Int(x, y, 0));

        HexCell cell = Instantiate(hexCellPrefab, spawnPos, Quaternion.identity, transform).GetComponent<HexCell>();
        targetGroupManager.AddTarget(cell.transform);
        cells.Add(new Vector2Int(x, y), cell);
        if(shouldInitStacks) cell.SpawnInitStacks();
        cell.gameObject.name = $"Cell ({x}, {y})";

        if(spawnPos.z < hexSpawnerZ) hexSpawnerZ = spawnPos.z;
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

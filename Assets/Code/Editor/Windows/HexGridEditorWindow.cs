using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.VisualScripting;
using System.Diagnostics.Eventing.Reader;

public class HexGridEditorWindow : EditorWindow
{
    private const float cellSize = 70f; // Size of each cell in pixels
    private const float cellOffsetX = 0.75f * cellSize; // X offset between cells in a row
    private const float cellOffsetY = 0.866025404f * cellSize; // Y offset between rows

    private int gridSize = 3; // Grid dimensions
    private bool isAxial = true; // Axial coordinate system
    private HexGridData hexGridData; // The ScriptableObject to save the data to
    private float maxDistance; // Maximum distance for axial grid

    private Grid grid; // Unity's Grid component

    private Dictionary<Vector2Int, GridCell> hexCells; // Store the hex cells

    private Vector2 scrollPosition = Vector2.zero;

    [MenuItem("Window/Hex Grid Editor")]
    public static void ShowWindow()
    {
        GetWindow<HexGridEditorWindow>("Hex Grid Editor");
    }

    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        ResetWindow();
    }

    private void ResetWindow()
    {
        hexCells?.Clear();
        hexCells ??= new();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Hex Grid Editor", EditorStyles.boldLabel);

        grid = (Grid)EditorGUILayout.ObjectField("Grid Component", grid, typeof(Grid), true);

        gridSize = EditorGUILayout.IntField("Grid Size", gridSize);
        isAxial = EditorGUILayout.Toggle("Is Axial", isAxial);
        if (GUILayout.Button("Generate Grid"))
        {
            GenerateGrid();
        }

        if (grid == null)
        {
            EditorGUILayout.HelpBox("Please assign a Grid component.", MessageType.Warning);
            return;
        }
        if (hexCells == null || hexCells.Count == 0) return;

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        // Display the hex grid
        for (int y = gridSize; y >= -gridSize; y--)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(cellOffsetX * Mathf.Abs(y));
            for (int x = -gridSize; x <= gridSize; x++)
            {
                if (!IsIndexValid(x, y)) continue;
                hexCells.TryGetValue(new(x, y), out GridCell cell);
                GUI.color = cell == null? Color.grey : cell.ShouldInitStacks ? Color.blue : Color.white;
                if (GUILayout.Button($"({x},{y})", GUILayout.Width(cellSize - 20)))
                {
                    if(cell != null) cell.ShouldInitStacks = !cell.ShouldInitStacks;
                }
                GUI.color = cell == null ? Color.grey : Color.white;
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    Vector2Int index = new(x, y);
                    if (hexCells.ContainsKey(index)) hexCells.Remove(index);
                    else
                    {
                        cell ??= new(index);
                        hexCells.Add(index, cell);
                    }
                }
                GUILayout.Space(cellOffsetX);
            }
            GUILayout.EndHorizontal(); GUILayout.Space(cellOffsetY);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.Space();

        hexGridData = (HexGridData)EditorGUILayout.ObjectField("Grid Data", hexGridData, typeof(HexGridData), false);
        if (GUILayout.Button("Save Grid Data"))
        {
            SaveGridData();
        }
        EditorGUILayout.Space();

        if (GUILayout.Button("Load Grid Data"))
        {
            LoadGridData();
        }
    }

    private void GenerateGrid()
    {
        if (grid == null) return;
        ResetWindow();

        maxDistance = grid.CellToWorld(Vector3Int.right).magnitude * gridSize;

        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++) CreateCell(x, y);
        }
    }
    private void CreateCell(int x, int y, GridCell cell = null)
    {
        Vector3 spawnPos = grid.CellToWorld(new Vector3Int(x, y, 0));
        if (isAxial && spawnPos.magnitude > maxDistance) return;
        Vector2Int cellIndex = new(x, y);

        cell ??= new(cellIndex);
        hexCells.Add(cellIndex, cell);
    }
    private void SaveGridData()
    {
        if (hexGridData != null)
        {
            hexGridData.hexCells = hexCells.Select(kvp => kvp.Value).ToList();
            EditorUtility.SetDirty(hexGridData);
            AssetDatabase.SaveAssets();
            Debug.Log("Hex grid data saved.");
        }
        else
        {
            Debug.LogWarning("Please assign a ScriptableObject to save the grid data.");
        }
    }
    private void LoadGridData()
    {
        ResetWindow();

        if (hexGridData != null && hexGridData.hexCells != null)
        {
            foreach (var cell in hexGridData.hexCells) CreateCell(cell.Index.x, cell.Index.y, cell);
            Debug.Log("Hex grid data loaded.");
        }
        else
        {
            Debug.LogWarning("No valid HexGridData to load.");
        }
    }
    private bool IsIndexValid(int x, int y)
    {

        Vector3 spawnPos = grid.CellToWorld(new Vector3Int(x, y, 0));
        if (isAxial && spawnPos.magnitude > maxDistance) return false;
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LevelData", menuName = "SO/Levels/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField, MinMaxSlider(1, 9)] private Vector2Int minMaxHexElementsCount;
    [SerializeField] private HexStack hexStackPrefab;
    [SerializeField] private HexGridData gridData;
    [SerializeField] private int epxPerStack = 2;
    public Vector2Int MinMaxHexElementsCount => minMaxHexElementsCount;
    public HexStack HexStackPrefab => hexStackPrefab;
    public HexGridData GridData => gridData;
    public int ExpPerElement => epxPerStack;

}

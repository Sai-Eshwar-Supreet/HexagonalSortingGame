using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new LevelData", menuName = "SO/Levels/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField, MinMaxSlider(1, 9)] private Vector2Int minMaxHexElementsCount;
    [SerializeField] private HexStack hexStackPrefab;
    public Vector2Int MinMaxHexElementsCount => minMaxHexElementsCount;
    public HexStack HexStackPrefab => hexStackPrefab;

}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPositionsTransform;

    private int availableStacksCount = 0;

    private void Awake()
    {
        StackMovementController.OnPlacedStack += StackPlacementHandler;
    }
    void Start()
    {
        SpawnStacks();
    }
    private void OnDestroy()
    {
        StackMovementController.OnPlacedStack -= StackPlacementHandler;
    }

    private void StackPlacementHandler(HexCell cell)
    {
        availableStacksCount--;
        if(availableStacksCount <= 0)
        {
            SpawnStacks();
        }
    }

    private void SpawnStacks()
    {
        HexStack stackPrefab = LevelManager.Instance.CurrentLevelData.HexStackPrefab;
        Vector2Int minMaxHexElementsCount = LevelManager.Instance.CurrentLevelData.MinMaxHexElementsCount;
        for (int i = 0; i < spawnPositionsTransform.childCount; i++)
        {
            Transform spawnTransform = spawnPositionsTransform.GetChild(i);
            HexStack hexStack =  Instantiate(stackPrefab, spawnTransform);
            int count = Random.Range(minMaxHexElementsCount.x, minMaxHexElementsCount.y + 1);

            hexStack.InstantiateElements(count);

            availableStacksCount++;
        }
    }
}

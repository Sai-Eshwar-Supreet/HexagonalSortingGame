using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HexSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPositionsTransform;
    [SerializeField] private TargetGroupManager targetGroupManager;

    private int availableStacksCount = 0;

    private void Awake()
    {
        StackMovementController.OnPlacedStack += StackPlacementHandler;
        
        for(int i = 0; i< spawnPositionsTransform.childCount; i++)
        {
            targetGroupManager.AddTarget(spawnPositionsTransform.GetChild(i));
        }
    }
    private void OnEnable()
    {
        SpawnStacks();
    }
    void Start()
    {
    }
    private void OnDestroy()
    {
        for (int i = 0; i < spawnPositionsTransform.childCount; i++)
        {
            targetGroupManager.RemoveTarget(spawnPositionsTransform.GetChild(i));
        }
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
        ClearStacks();
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
    private void ClearStacks()
    {
        for (int i = 0; i < spawnPositionsTransform.childCount; i++)
        {
            Transform spawnTransform = spawnPositionsTransform.GetChild(i);
            spawnTransform.Clear();
        }
        availableStacksCount = 0;
    }
}

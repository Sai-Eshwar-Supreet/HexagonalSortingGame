using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HexSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPositionsTransform;
    [SerializeField] private HexElement hexElementPrefab;
    [SerializeField] private HexStack hexStackPrefab;
    
    [SerializeField, MinMaxSlider(1,9)] private Vector2Int minMaxHexElementsCount;
    [SerializeField] private List<Material> materials;

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
        for (int i = 0; i < spawnPositionsTransform.childCount; i++)
        {
            Transform spawnTransform = spawnPositionsTransform.GetChild(i);
            HexStack hexStack =  Instantiate(hexStackPrefab, spawnTransform);
            int count = Random.Range(minMaxHexElementsCount.x, minMaxHexElementsCount.y + 1);



            int[] selectedMatsIndices = new int[] { GetRandomMaterialIndex(), GetRandomMaterialIndex(), GetRandomMaterialIndex() };
            int changePoint1, changePoint2;

            changePoint1 = Random.Range(0, count);
            changePoint2 = changePoint1 + Random.Range(0, count - changePoint1);

            for(int j = 0; j < count; j++)
            {
                HexElement element = Instantiate(hexElementPrefab, hexStack.transform);
                hexStack.Add(element);
                int selectedIndex = selectedMatsIndices[(j < changePoint1) ? 0 : (j<changePoint2) ? 1 : 2];
                element.SetMaterial(materials[selectedIndex], selectedIndex);
            }
            availableStacksCount++;
        }
    }
    private int GetRandomMaterialIndex()
    {
        return Random.Range(0, materials.Count);
    }
}

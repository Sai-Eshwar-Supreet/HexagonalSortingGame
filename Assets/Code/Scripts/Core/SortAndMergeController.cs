using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SortAndMergeController : MonoBehaviour
{
    private readonly List<HexCell> modifiedCells = new();

    private void Awake()
    {
        StackMovementController.OnPlacedStack += MergeHandler;
    }
    private void OnDestroy()
    {
        StackMovementController.OnPlacedStack -= MergeHandler;
    }

    private void MergeHandler(HexCell cell)
    {
        StartCoroutine(MergeCoroutine(cell));

    }

    private IEnumerator MergeCoroutine(HexCell cell)
    {
        modifiedCells.Add(cell);
        while (modifiedCells.Count > 0)
        {
            yield return CheckForSortNMerge(modifiedCells[0]);
        }
    }

    private IEnumerator CheckForSortNMerge(HexCell cell)
    {
        modifiedCells.Remove(cell);
        if (cell == null || !cell.IsOccupied) yield break;

        int topIndex = cell.OccupantStack.TopHexagonIndex;

        List<HexCell> neighbors = HexGridGenerator.GetNeighbors_Static(cell);

        List<HexCell> validNeighbors = neighbors.Where(
            val => (val.IsOccupied && val.OccupantStack.Elements.Count > 0 && (val.OccupantStack.TopHexagonIndex == topIndex))).ToList();

        if (validNeighbors == null || validNeighbors.Count == 0) yield break;

        modifiedCells.AddRange(validNeighbors);

        List<HexElement> poppedElements = new();

        foreach (HexCell neighbor in validNeighbors)
        {
            HexStack stack = neighbor.OccupantStack;

            if (stack == null) continue;
            while (stack.Elements.Count > 0 && stack.Peek().MaterialIndex == topIndex)
            {
                var element = stack.Pop();
                poppedElements.Add(element);
                element.SetParent(null);
            }
            if (stack.Elements.Count == 0) stack.DestroyStack();
        }

        foreach (var element in poppedElements)
        {
            cell.OccupantStack.Add(element, true);
            yield return new WaitForSeconds(0.15f);
        }

        yield return CheckForStackMerge(cell);
    }
    private IEnumerator CheckForStackMerge(HexCell cell)
    {
        HexStack hexStack = cell.OccupantStack;
        if (hexStack.Elements.Count < 10) yield break;

        int topIndex = hexStack.TopHexagonIndex;

        int count = 0;

        foreach(var element in hexStack.Elements)
        {
            if (element.MaterialIndex != topIndex) break;
            count++;
        }

        if (count < 10) yield break;

        GameManager.Instance.AddExp(count * LevelManager.Instance.CurrentLevelData.ExpPerElement);

        while (hexStack.Elements.Count > 0 && hexStack.Peek().MaterialIndex == topIndex)
        {
            var element = hexStack.Pop();
            element.SetParent(null);
            element.DestroySelf();
            yield return new WaitForSeconds(0.05f);
        }
        if (hexStack.Elements.Count == 0) hexStack.DestroyStack();
        modifiedCells.Add(cell);
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexStack : MonoBehaviour
{
    [SerializeField] private HexElement hexElementPrefab;
    [SerializeField] private List<Material> materials;
    public Stack<HexElement> Elements { get; private set; }

    public void Add(HexElement element)
    {
        Elements ??= new();
        Elements.Push(element);
        element.Configure(this);
        element.SetParent(transform);
        element.LocalMove(GetNewElementPositon(Elements.Count - 1));

    }

    public void DisableInteraction()
    {
        foreach (HexElement element in Elements) element.DisableCollider();
    }

    public int TopHexagonIndex => Peek().MaterialIndex;

    public HexElement Peek() => Elements.Peek();
    public HexElement Pop() => Elements.Pop();
    public void DestroyStack() => Destroy(gameObject);

    private Vector3 GetNewElementPositon(int index)
    {
        float height = 0.1f;
        return (0.5f + index) * height * Vector3.up;
    }

    public void InstantiateElements(int count)
    {
        int[] selectedMatsIndices = new int[] { GetRandomMaterialIndex(), GetRandomMaterialIndex(), GetRandomMaterialIndex() };
        int changePoint1, changePoint2;

        changePoint1 = Random.Range(0, count);
        changePoint2 = changePoint1 + Random.Range(0, count - changePoint1);

        for (int j = 0; j < count; j++)
        {
            HexElement element = Instantiate(hexElementPrefab, transform);
            Add(element);
            int selectedIndex = selectedMatsIndices[(j < changePoint1) ? 0 : (j < changePoint2) ? 1 : 2];
            element.SetMaterial(materials[selectedIndex], selectedIndex);
        }
    }

    private int GetRandomMaterialIndex()
    {
        return Random.Range(0, materials.Count);
    }
}

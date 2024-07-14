using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexStack : MonoBehaviour
{ 
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
}

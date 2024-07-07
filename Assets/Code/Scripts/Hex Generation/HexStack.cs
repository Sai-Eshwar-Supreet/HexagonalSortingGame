using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class HexStack : MonoBehaviour
{
    [SerializeField] private GameObject hexElement;
    [SerializeField] private HexGrid grid;
    [SerializeField] private Material[] materials;

    private readonly Stack<GameObject> stack = new();


    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    void Start()
    {
        int count = Random.Range(1, 7);
        SetUpCollider(count);

        stack.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject element = Instantiate(hexElement, transform);
            SetupElement(element, GetPositon(i));
            stack.Push(element);
        }
    }
    private void SetupElement(GameObject element, Vector3 position)
    {
        element.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial();
        element.transform.localPosition = position;

        float hexDiameter = grid.Configuration.OuterRadius * 2;

        element.transform.localScale = Vector3.one * hexDiameter;
        element.transform.localRotation = (grid.Configuration.IsFlatTopped) ?
            Quaternion.Euler(-90, 0, 90) : Quaternion.Euler(-90, 0, 0);
        element.transform.localPosition = position;
    }
    private void SetUpCollider(int hexCount)
    {
        Vector3 size, center;
        size.x = (grid.Configuration.IsFlatTopped ? 2 * grid.Configuration.OuterRadius : (1.732f * grid.Configuration.OuterRadius));
        size.z = (!grid.Configuration.IsFlatTopped ? 2 * grid.Configuration.OuterRadius : (1.732f * grid.Configuration.OuterRadius));
        size.y = hexCount * 0.2f;
        center = Vector3.up * size.y / 2;

        boxCollider.size = size;
        boxCollider.center = center;

    }
    private Vector3 GetPositon(int index)
    {
        float height = 0.2f;
        return (0.5f + index) * height * Vector3.up;
    }

    private Material GetMaterial()
    {
        int random = Random.Range(0, materials.Length);
        return materials[random];
    }
}

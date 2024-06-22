using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HexGridLayout : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private GridConfiguration configuration;

    private readonly Dictionary<(int a, int b), GameObject> gridElements = new();

    public void SwitchConfiguration(GridConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public IEnumerator ClearGrid()
    {
        foreach(var element in gridElements.Values) Destroy(element);
        gridElements.Clear();
        yield return null;
    }

    public IEnumerator SetupGrid()
    {
        if (configuration.IsAxial) CreateAxialGrid();
        else CreateOffsetGrid();
        yield return null;
    }

    private void CreateOffsetGrid()
    {
        int boundX = configuration.ColumnSize - 1;
        int boundY = configuration.RowSize - 1;
        for (int x = -boundX; x <= boundX; x++)
        {
            for (int y = -boundY; y <=boundY; y++)
            {
                GameObject element = new($"{x}, {y}", typeof(HexRenderer));
                gridElements.Add((x, y), element);
                element.transform.position = GetPositon(x, y);

                SetupRenderer(element);
            }
        }
    }

    private void CreateAxialGrid()
    {
        int bound = configuration.AxialSize - 1;
        for (int r = -bound; r <= bound; r++)
        {
            int start = Mathf.Max(-bound - r, -bound);
            int end = Mathf.Min(bound - r, bound);
            for (int q = start; q <= end; q++)
            {
                GameObject element = new($"{r}, {q}", typeof(HexRenderer));
                gridElements.Add((r, q), element);

                element.transform.position = GetAxialPositon(r, q);

                SetupRenderer(element);
            }
        }
    }

    private void SetupRenderer(GameObject element)
    {
        HexRenderer hexRenderer = element.GetComponent<HexRenderer>();
        hexRenderer.InnerRadius = configuration.InnerRadius;
        hexRenderer.OuterRadius = configuration.OuterRadius;
        hexRenderer.Height = configuration.Height;
        hexRenderer.IsFlatTopped = configuration.IsFlatTopped;
        hexRenderer.SetMaterial(material);
        hexRenderer.DrawMesh();

        element.transform.SetParent(transform, true);
    }
    private Vector3 GetAxialPositon(int r, int q)
    {
        float size = configuration.OuterRadius;
        float x;
        float y;

        if (configuration.IsFlatTopped)
        {
            x = size * (3f / 2f) * q;
            y = size * Mathf.Sqrt(3) * (r + q / 2f);
        }
        else
        {
            x = size * Mathf.Sqrt(3) * (q + r / 2f);
            y = size * (3f / 2f) * r;
        }

        return new Vector3(x, 0, y);
    }
    private Vector3 GetPositon(int column, int row)
    {
        bool shouldOffset;
        float width, height;
        float horizontalDistance, verticalDistance;
        float xPosition, yPosition;
        float size = configuration.OuterRadius;

        if (!configuration.IsFlatTopped)
        {
            shouldOffset = (row % 2 == 0);

            width = Mathf.Sqrt(3) * size;
            height = 2 * size;

            horizontalDistance = width;
            verticalDistance = (3f / 4f) * height;

            float offset = shouldOffset ? width / 2f : 0;

            xPosition = (column * horizontalDistance) + offset;
            yPosition = row * verticalDistance;
        }
        else
        {
            shouldOffset = (column % 2 == 0);

            width = 2 * size;
            height = Mathf.Sqrt(3) * size;

            horizontalDistance = (3f / 4f) * width;
            verticalDistance = height;

            float offset = shouldOffset ? height / 2f : 0;

            xPosition = column * horizontalDistance;
            yPosition = (row * verticalDistance) - offset;
        }

        return new Vector3(xPosition, 0, -yPosition);
    }
}

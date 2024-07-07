using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[DisallowMultipleComponent]
public class HexRenderer : MonoBehaviour
{
    public float InnerRadius { get; set; }
    public float OuterRadius { get; set; }
    public float Height { get; set; }
    public bool IsFlatTopped { get; set; }

    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private List<Face> faces = new();

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        mesh = new() { name = "Hexagon" };

        meshFilter.sharedMesh = mesh;
    }

    public void SetMaterial(Material material)
    {
        meshRenderer.sharedMaterial = material;
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    private void DrawFaces()
    {
        faces = new List<Face>();
        OuterRadius = Mathf.Max(InnerRadius, OuterRadius);
        for (int i = 0; i < 6; i++)
        {
            //top faces
            faces.Add(CreateFace(InnerRadius, OuterRadius, Height / 2, Height / 2, i));
            //bottom faces
            faces.Add(CreateFace(InnerRadius, OuterRadius, -Height / 2, -Height / 2, i, true));
            //outer faces
            faces.Add(CreateFace(OuterRadius, OuterRadius, Height / 2, -Height / 2, i, true));
            //inner faces
            faces.Add(CreateFace(InnerRadius, InnerRadius, Height / 2, -Height / 2, i));


        }
    }


    private Face CreateFace(float innerRadius, float outerRadius, float heightA,
        float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRadius, heightB, point);
        Vector3 pointB = GetPoint(innerRadius, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRadius, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRadius, heightA, point);

        List<Vector3> verts = new() { pointA, pointB, pointC, pointD };
        List<int> tris = new() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new() { Vector2.zero, Vector2.right, Vector2.one, Vector2.up };

        if (reverse) verts.Reverse();

        return new Face(verts, tris, uvs);

    }

    private Vector3 GetPoint(float size, float height, int index)
    {
        float angle = (IsFlatTopped ? index * 60 : index * 60 - 30) * Mathf.Deg2Rad;

        return new Vector3(size * Mathf.Cos(angle), height,
            size * Mathf.Sin(angle));
    }
    private void CombineFaces()
    {
        List<Vector3> verts = new();
        List<int> tris = new();
        List<Vector2> uvs = new();

        for (int i = 0; i < faces.Count; i++)
        {
            verts.AddRange(faces[i].Verts);
            uvs.AddRange(faces[i].UVs);

            int offset = 4 * i;
            foreach (var tri in faces[i].Tris)
            {
                tris.Add(tri + offset);
            }
        }
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();
    }
}

public struct Face
{
    public List<Vector3> Verts { get; private set; }
    public List<int> Tris { get; private set; }
    public List<Vector2> UVs { get; private set; }

    public Face(List<Vector3> verts, List<int> tris, List<Vector2> uvs)
    {
        Verts = verts;
        Tris = tris;
        UVs = uvs;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class VertexClustering : MonoBehaviour
{
    [SerializeField] private float epsilon = 0.5f;

    private Mesh meshOriginal;
    private Mesh meshSimplified;

    private Vector3 minBounds;
    private Dictionary<Vector3Int, List<int>> grid;
    private Dictionary<Vector3Int, int> cellToNewIndex;
    private List<Vector3> newVertices;
    private List<int> newTriangles;

    void Start()
    {
        meshOriginal = GetComponent<MeshFilter>().sharedMesh;
        meshSimplified = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = meshSimplified;

        BuildGrid();
        CreateRepresentativeVertices();
        ReconstructGeometry();

        meshSimplified.RecalculateNormals();
        meshSimplified.RecalculateBounds();
    }

    void BuildGrid()
    {
        grid = new Dictionary<Vector3Int, List<int>>();

        Vector3[] verts = meshOriginal.vertices;

        minBounds = verts[0];
        Vector3 maxBounds = verts[0];
        foreach (var v in verts)
        {
            minBounds = Vector3.Min(minBounds, v);
            maxBounds = Vector3.Max(maxBounds, v);
        }

        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 v = verts[i];
            Vector3Int cell = WorldToCell(v);

            if (!grid.ContainsKey(cell))
            {
                grid[cell] = new List<int>();
            }

            grid[cell].Add(i);
        }
    }

    Vector3Int WorldToCell(Vector3 v)
    {
        return new Vector3Int(
            Mathf.FloorToInt((v.x - minBounds.x) / epsilon),
            Mathf.FloorToInt((v.y - minBounds.y) / epsilon),
            Mathf.FloorToInt((v.z - minBounds.z) / epsilon)
        );
    }

    void CreateRepresentativeVertices()
    {
        newVertices = new List<Vector3>();
        cellToNewIndex = new Dictionary<Vector3Int, int>();

        Vector3[] verts = meshOriginal.vertices;

        foreach (var entry in grid)
        {
            Vector3Int cell = entry.Key;
            List<int> list = entry.Value;

            Vector3 moyenne = Vector3.zero;
            foreach (int i in list)
            {
                moyenne += verts[i];
            }

            moyenne /= list.Count;

            cellToNewIndex[cell] = newVertices.Count;
            newVertices.Add(moyenne);
        }

    }

    void ReconstructGeometry()
    {
        int[] oldTriangles = meshOriginal.triangles;
        Vector3[] oldVertices = meshOriginal.vertices;

        newTriangles = new List<int>();

        for (int i = 0; i < oldTriangles.Length; i += 3)
        {
            int a = oldTriangles[i];
            int b = oldTriangles[i + 1];
            int c = oldTriangles[i + 2];

            Vector3Int ca = WorldToCell(oldVertices[a]);
            Vector3Int cb = WorldToCell(oldVertices[b]);
            Vector3Int cc = WorldToCell(oldVertices[c]);

            int na = cellToNewIndex[ca];
            int nb = cellToNewIndex[cb];
            int nc = cellToNewIndex[cc];

            if (na == nb || nb == nc || na == nc)
            {
                continue;
            }

            newTriangles.Add(na);
            newTriangles.Add(nb);
            newTriangles.Add(nc);
        }

        meshSimplified.Clear();
        meshSimplified.vertices = newVertices.ToArray();
        meshSimplified.triangles = newTriangles.ToArray();
    }
}

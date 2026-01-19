using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class Faces
{
    public int One, Two, Three;
    public Faces(int one, int two, int three)
    {
        One = one;
        Two = two;
        Three = three;
    }
}

public class offStructure
{
    Mesh mesh;

    const string OFFMagicNumber = "OFF";
    uint nbVertices = 0;
    uint nbFacette = 0;

    public List<Vector3> Vertices = new List<Vector3>();
    public List<Faces> Faces = new List<Faces>();

    Dictionary<(int, int), List<int>> edgeOpposites;

    public offStructure(string filePath, Mesh msh)
    {
        mesh = msh;
        LoadFromOffFile(filePath);
        ConstructObject();
    }

    public void LoadFromOffFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line = reader.ReadLine();
            if (line != OFFMagicNumber)
                throw new ArgumentException("Ce n'est pas un fichier OFF");

            line = reader.ReadLine();
            string[] lineSplited = line.Split(' ');

            nbVertices = uint.Parse(lineSplited[0]);
            nbFacette = uint.Parse(lineSplited[1]);

            for (int i = 0; i < nbVertices; ++i)
            {
                line = reader.ReadLine();
                lineSplited = line.Split(' ');
                Vertices.Add(new Vector3(
                    float.Parse(lineSplited[0], CultureInfo.InvariantCulture),
                    float.Parse(lineSplited[1], CultureInfo.InvariantCulture),
                    float.Parse(lineSplited[2], CultureInfo.InvariantCulture)
                ));
            }

            for (int i = 0; i < nbFacette; ++i)
            {
                line = reader.ReadLine();
                lineSplited = line.Split(' ');
                if (int.Parse(lineSplited[0]) != 3)
                    throw new ArgumentException("Face non triangulaire");

                Faces.Add(new Faces(
                    int.Parse(lineSplited[1]),
                    int.Parse(lineSplited[2]),
                    int.Parse(lineSplited[3])
                ));
            }
        }
    }

    public void ConstructObject()
    {
        mesh.Clear();
        mesh.vertices = Vertices.ToArray();

        int[] tris = new int[Faces.Count * 3];
        int t = 0;
        foreach (var f in Faces)
        {
            tris[t++] = f.One;
            tris[t++] = f.Two;
            tris[t++] = f.Three;
        }

        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public void CentreGravite()
    {
        Vector3 center = Vector3.zero;
        foreach (var v in Vertices)
            center += v;
        center /= Vertices.Count;

        for (int i = 0; i < Vertices.Count; i++)
            Vertices[i] -= center;

        mesh.vertices = Vertices.ToArray();
        mesh.RecalculateBounds();
    }

    public void Normalize()
    {
        float max = 0f;
        foreach (var v in Vertices)
            max = Mathf.Max(max, Mathf.Max(Mathf.Abs(v.x), Mathf.Max(Mathf.Abs(v.y), Mathf.Abs(v.z))));

        for (int i = 0; i < Vertices.Count; i++)
            Vertices[i] /= max;

        mesh.vertices = Vertices.ToArray();
        mesh.RecalculateBounds();
    }

    public void Normale()
    {
        Vector3[] normals = new Vector3[Vertices.Count];

        foreach (var f in Faces)
        {
            Vector3 A = Vertices[f.One];
            Vector3 B = Vertices[f.Two];
            Vector3 C = Vertices[f.Three];

            Vector3 N = Vector3.Cross(B - A, C - A).normalized;
            normals[f.One] += N;
            normals[f.Two] += N;
            normals[f.Three] += N;
        }

        for (int i = 0; i < normals.Length; i++)
            normals[i] = normals[i].normalized;

        mesh.normals = normals;
    }

    public void LoopSubdivision()
    {
        // Champ edgeOpposites déjà déclaré comme champ de classe
        edgeOpposites = new Dictionary<(int, int), List<int>>();
        Dictionary<(int, int), int> edgeIndex = new Dictionary<(int, int), int>();
        Dictionary<int, HashSet<int>> neighbors = new Dictionary<int, HashSet<int>>();

        for (int i = 0; i < Vertices.Count; i++)
            neighbors[i] = new HashSet<int>();

        foreach (var f in Faces)
        {
            RegisterEdge(f.One, f.Two, f.Three);
            RegisterEdge(f.Two, f.Three, f.One);
            RegisterEdge(f.Three, f.One, f.Two);

            neighbors[f.One].Add(f.Two);
            neighbors[f.One].Add(f.Three);
            neighbors[f.Two].Add(f.One);
            neighbors[f.Two].Add(f.Three);
            neighbors[f.Three].Add(f.One);
            neighbors[f.Three].Add(f.Two);
        }

        List<Vector3> newVertices = new List<Vector3>(Vertices);

        foreach (var e in edgeOpposites)
        {
            int i1 = e.Key.Item1;
            int i2 = e.Key.Item2;
            int i3 = e.Value[0];
            int i4 = e.Value.Count > 1 ? e.Value[1] : i3;

            Vector3 p =
                (3f / 8f) * (Vertices[i1] + Vertices[i2]) +
                (1f / 8f) * (Vertices[i3] + Vertices[i4]);

            edgeIndex[e.Key] = newVertices.Count;
            newVertices.Add(p);
        }

        Vector3[] updated = new Vector3[Vertices.Count];
        for (int i = 0; i < Vertices.Count; i++)
        {
            int n = neighbors[i].Count;
            float beta = (n == 3) ? 3f / 16f : 3f / (8f * n);

            Vector3 sum = Vector3.zero;
            foreach (int j in neighbors[i])
                sum += Vertices[j];

            updated[i] = (1 - n * beta) * Vertices[i] + beta * sum;
        }

        for (int i = 0; i < Vertices.Count; i++)
            newVertices[i] = updated[i];

        List<Faces> newFaces = new List<Faces>();
        foreach (var f in Faces)
        {
            int A = f.One;
            int B = f.Two;
            int C = f.Three;

            int AB = edgeIndex[Key(A, B)];
            int BC = edgeIndex[Key(B, C)];
            int CA = edgeIndex[Key(C, A)];

            newFaces.Add(new Faces(A, AB, CA));
            newFaces.Add(new Faces(AB, B, BC));
            newFaces.Add(new Faces(CA, BC, C));
            newFaces.Add(new Faces(AB, BC, CA));
        }

        Vertices = newVertices;
        Faces = newFaces;

        ConstructObject();
    }

    private void RegisterEdge(int a, int b, int opposite)
    {
        var key = Key(a, b);
        if (!edgeOpposites.ContainsKey(key))
            edgeOpposites[key] = new List<int>();
        edgeOpposites[key].Add(opposite);
    }

    private (int, int) Key(int a, int b)
    {
        return (Math.Min(a, b), Math.Max(a, b));
    }
}

public class off : MonoBehaviour
{
    [SerializeField] private string nomFichier;
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        string path = Path.Combine(Application.dataPath, "mesh/" + nomFichier + ".off");
        offStructure structure = new offStructure(path, mesh);

        structure.CentreGravite();
        structure.Normalize();

        structure.LoopSubdivision();

        structure.ConstructObject();
    }
}

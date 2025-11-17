using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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
    public offStructure(string filePath, Mesh msh)
    {
        mesh = msh;
        LoadFromOffFile(filePath);
        ConstructObject();
    }
    Mesh mesh;
    const string OFFMagicNumber = "OFF";
    uint nbVertices = 0;
    uint nbFacette = 0;
    //uint nbArrete = 0;
    public List<Vector3> Vertices = new List<Vector3>();
    public List<Faces> Faces = new List<Faces>();

    public void LoadFromOffFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line = reader.ReadLine();
            if (line != OFFMagicNumber)
            {
                throw new ArgumentException("Ce n'est pas un fichier OFF");
            }
            line = reader.ReadLine();
            string[] lineSplited = line.Split(' ');
            nbVertices = uint.Parse(lineSplited[0]);
            nbFacette = uint.Parse(lineSplited[1]);
            // nbArrete = uint.Parse(lineSplited[2]);

            for (int i = 0; i < nbVertices; ++i)
            {
                line = reader.ReadLine();
                lineSplited = line.Split(' ');
                Vertices.Add(new Vector3(float.Parse(lineSplited[0], CultureInfo.InvariantCulture), float.Parse(lineSplited[1], CultureInfo.InvariantCulture), float.Parse(lineSplited[2], CultureInfo.InvariantCulture)));
            }

            for (int i = 0; i < nbFacette; ++i)
            {
                line = reader.ReadLine();
                lineSplited = line.Split(' ');
                if (int.Parse(lineSplited[0]) != 3)
                {
                    throw new ArgumentException("Une des faces n'est pas composé de 3 sommets.");
                }
                Faces.Add(new Faces(int.Parse(lineSplited[1]), int.Parse(lineSplited[2]), int.Parse(lineSplited[3])));
            }
        }
    }

    public void ConstructObject()
    {
        Vector3[] verts = Vertices.ToArray();

        int[] tris = new int[Faces.Count * 3];
        int ti = 0;

        for (int i = 0; i < Faces.Count; i++)
        {
            tris[ti++] = Faces[i].One;
            tris[ti++] = Faces[i].Two;
            tris[ti++] = Faces[i].Three;
        }

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }


    public void CentreGravite()
    {
        Vector3 result = new Vector3();
        for(int i=0; i < nbVertices; ++i)
        {
            result += Vertices[i];
        }
        result/= nbVertices;

        for (int i = 0; i < nbVertices; ++i)
        {
            Vertices[i] -= result;
        }


        mesh.vertices = Vertices.ToArray();
        mesh.RecalculateBounds();

    }

    public void Normalize()
    {
        float max = 1;
        float temp = 0;
        for (int i = 0; i < nbVertices; ++i)
        {
            temp = Math.Max(Math.Abs(Vertices[i].x), Math.Max(Math.Abs(Vertices[i].y), Math.Abs(Vertices[i].z)));
            if (temp > max)
            {
                max = temp;
            }
        }

        for (int i = 0; i < nbVertices; ++i)
        {
            Vertices[i] /= max;
        }

        mesh.vertices = Vertices.ToArray();
        mesh.RecalculateBounds();
    }

    public void Normale()
    {
        Vector3[] normals = new Vector3[Vertices.Count];

        for (int i = 0; i < normals.Length; i++)
        { 
            normals[i] = Vector3.zero;
        }

        for (int i = 0; i < Faces.Count; i++)
        {
            int iA = Faces[i].One;
            int iB = Faces[i].Two;
            int iC = Faces[i].Three;

            Vector3 A = Vertices[iA];
            Vector3 B = Vertices[iB];
            Vector3 C = Vertices[iC];

            Vector3 AB = B - A;
            Vector3 AC = C - A;

            Vector3 N = Vector3.Cross(AB, AC).normalized;

            normals[iA] += N;
            normals[iB] += N;
            normals[iC] += N;
        }

        for (int i = 0; i < normals.Length; i++)
        { 
            normals[i] = normals[i].normalized;
        }

        mesh.normals = normals;
    }

    public void ExportToOffFile(string filePath)
    {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("OFF");
                writer.WriteLine($"{Vertices.Count} {Faces.Count} 0");
                foreach (var v in Vertices)
                    writer.WriteLine($"{v.x.ToString(CultureInfo.InvariantCulture)} " +
                                     $"{v.y.ToString(CultureInfo.InvariantCulture)} " +
                                     $"{v.z.ToString(CultureInfo.InvariantCulture)}");
                foreach (var f in Faces)
                    writer.WriteLine($"3 {f.One} {f.Two} {f.Three}");
            }
    }

    public void RemoveNFaces(int n)
    {
        if (n > Faces.Count) n = Faces.Count;

        Faces.RemoveRange(Faces.Count - n, n);
        nbFacette = (uint)Faces.Count;

        HashSet<int> used = new HashSet<int>();

        foreach (var f in Faces)
        {
            used.Add(f.One);
            used.Add(f.Two);
            used.Add(f.Three);
        }

        List<Vector3> newVertices = new List<Vector3>();
        Dictionary<int, int> indexMap = new Dictionary<int, int>();

        int newIndex = 0;
        for (int i = 0; i < Vertices.Count; i++)
        {
            if (used.Contains(i))
            {
                newVertices.Add(Vertices[i]);
                indexMap[i] = newIndex;
                newIndex++;
            }
        }

        foreach (var f in Faces)
        {
            f.One = indexMap[f.One];
            f.Two = indexMap[f.Two];
            f.Three = indexMap[f.Three];
        }

        Vertices = newVertices;

        ConstructObject();
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

        structure.RemoveNFaces(2);

        string export = Path.Combine(Application.dataPath, "Exports/export_" + nomFichier + ".off");
        structure.ExportToOffFile(export);
    }

}

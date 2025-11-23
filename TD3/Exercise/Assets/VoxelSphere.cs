using System;
using System.Collections.Generic;
using UnityEngine;

public class SpatialEnumTP3 : MonoBehaviour
{
    [Serializable]
    public class SphereTP3
    {
        public Vector3 centre = Vector3.zero;
        public float rayon = 0.5f;
    }
    public enum Operation { Union, Intersection }

    public int resolution = 20;
    public Operation operateur = Operation.Union;
    public List<SphereTP3> spheres = new List<SphereTP3>();

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        if (spheres.Count == 0)
        {
            return;
        }

        // Calculate bounding box
        Bounds box = new Bounds(spheres[0].centre, Vector3.zero);

        foreach (var sphere in spheres)
        {
            Vector3 min = sphere.centre - Vector3.one * sphere.rayon;
            Vector3 max = sphere.centre + Vector3.one * sphere.rayon;

            box.Encapsulate(min);
            box.Encapsulate(max);
        }


        // Construct
        float pas = box.size.x / resolution;

        List<Vector3> centres = new List<Vector3>();

        Vector3 debut = box.min + Vector3.one * (pas * 0.5f);

        int nx = Mathf.CeilToInt(box.size.x / pas);
        int ny = Mathf.CeilToInt(box.size.y / pas);
        int nz = Mathf.CeilToInt(box.size.z / pas);

        for (int ix = 0; ix < nx; ix++)
        {
            for (int iy = 0; iy < ny; iy++)
            {
                for (int iz = 0; iz < nz; iz++)
                {
                    // Is on the bound?
                    Vector3 p = debut + new Vector3(ix * pas, iy * pas, iz * pas);

                    bool premier = true;
                    bool resultat = false;

                    foreach (var sphere in spheres)
                    {
                        float d = Vector3.Distance(p, sphere.centre);
                        bool dedans = d <= sphere.rayon;

                        if (premier)
                        {
                            resultat = dedans;
                            premier = false;
                        }
                        else
                        {
                            if (operateur == Operation.Union) // Union
                            {
                                resultat = resultat || dedans;
                            }
                            else // Intersection
                            {
                                resultat = resultat && dedans;
                            }
                        }
                    }

                    if (resultat)
                    {
                        centres.Add(p);
                    }
                }
            }
        }

        mesh.Clear();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        Vector3 h = Vector3.one * (pas * 0.5f);

        int baseIndex = 0;

        foreach (var centre in centres)
        {
            verts.Add(centre + new Vector3(-h.x, -h.y, -h.z));
            verts.Add(centre + new Vector3(h.x, -h.y, -h.z));
            verts.Add(centre + new Vector3(h.x, h.y, -h.z));
            verts.Add(centre + new Vector3(-h.x, h.y, -h.z));

            verts.Add(centre + new Vector3(-h.x, -h.y, h.z));
            verts.Add(centre + new Vector3(h.x, -h.y, h.z));
            verts.Add(centre + new Vector3(h.x, h.y, h.z));
            verts.Add(centre + new Vector3(-h.x, h.y, h.z));

            int[] t = new int[]
            {
                0,1,2, 0,2,3,
                5,4,7, 5,7,6,
                4,5,1, 4,1,0,
                3,2,6, 3,6,7,
                1,5,6, 1,6,2,
                4,0,3, 4,3,7
            };

            for (int i = 0; i < t.Length; i++)
            {
                tris.Add(baseIndex + t[i]);
            }

            baseIndex += 8;
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}

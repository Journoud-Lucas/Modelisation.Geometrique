using UnityEngine;

public class triangle : MonoBehaviour
{
    [SerializeField] private int nbLignes;
    [SerializeField] private int nbColonnes;
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int totalTriangles = nbLignes * nbColonnes * 2;
        Vector3[] vertices = new Vector3[totalTriangles * 3];
        int[] triangles = new int[totalTriangles * 3];

        int v = 0;
        int t = 0;

        for (int ligne = 0; ligne < nbLignes; ligne++)
        {
            for (int colonne = 0; colonne < nbColonnes; ++colonne)
            {
                vertices[v + 0] = new Vector3(colonne, ligne, 0);
                vertices[v + 1] = new Vector3(colonne + 1, ligne, 0);
                vertices[v + 2] = new Vector3(colonne, ligne + 1, 0);

                vertices[v + 3] = new Vector3(colonne + 1, ligne + 1, 0);
                vertices[v + 4] = new Vector3(colonne, ligne + 1, 0);
                vertices[v + 5] = new Vector3(colonne + 1, ligne, 0);

                triangles[t + 0] = v + 0;
                triangles[t + 1] = v + 1;
                triangles[t + 2] = v + 2;
                triangles[t + 3] = v + 3;
                triangles[t + 4] = v + 4;
                triangles[t + 5] = v + 5;
                v += 6;
                t += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    void Update()
    {

    }
}

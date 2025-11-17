using UnityEngine;

public class Cone : MonoBehaviour
{
    [SerializeField] private float rayon = 1f;
    [SerializeField] private float hauteur = 2f;
    [SerializeField] private int nbMeridiens = 36;
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int nbVerticesBase = nbMeridiens + 1;
        int nbVerticesTotal = nbVerticesBase + 1;

        Vector3[] vertices = new Vector3[nbVerticesTotal];
        int[] triangles = new int[nbMeridiens * 3 * 2];

        vertices[0] = new Vector3(0, 0, 0); // Centre de la base
        for (int i = 0; i < nbMeridiens; i++)
        {
            float angle = i * Mathf.PI * 2 / nbMeridiens;
            vertices[i + 1] = new Vector3(rayon * Mathf.Cos(angle), 0, rayon * Mathf.Sin(angle));
        }

        vertices[nbVerticesBase] = new Vector3(0, hauteur, 0); // Pointe du cône

        for (int i = 0; i < nbMeridiens; i++)
        {
            int next = (i + 1) % nbMeridiens;
            // Triangles pour la base
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = next + 1;

            // Triangles pour la surface latérale
            triangles[(i + nbMeridiens) * 3] = i + 1;
            triangles[(i + nbMeridiens) * 3 + 1] = nbVerticesBase;
            triangles[(i + nbMeridiens) * 3 + 2] = next + 1;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}

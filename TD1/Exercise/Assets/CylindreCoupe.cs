using UnityEngine;

public class CylindreCoupe : MonoBehaviour
{
    [SerializeField] private float rayon = 1f;
    [SerializeField] private float hauteur = 2f;
    [SerializeField] private int nbMeridiens = 36;
    [SerializeField] private int nbDivise = 1;
    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int nbVerticesBase = nbMeridiens + 1;
        int nbVerticesLat = nbMeridiens * 2;
        int nbVerticesTotal = nbVerticesBase * 2 + nbMeridiens * 2;

        Vector3[] vertices = new Vector3[nbVerticesTotal];
        int[] triangles = new int[nbMeridiens * 12];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < nbMeridiens; i++)
        {
            float angle = i * Mathf.PI * 2 / nbMeridiens;
            vertices[i + 1] = new Vector3(rayon * Mathf.Cos(angle), 0, rayon * Mathf.Sin(angle));
        }

        vertices[nbMeridiens + 1] = new Vector3(0, hauteur, 0);
        for (int i = 0; i < nbMeridiens; i++)
        {
            float angle = i * Mathf.PI * 2 / nbMeridiens;
            vertices[nbMeridiens + 2 + i] = new Vector3(rayon * Mathf.Cos(angle), hauteur, rayon * Mathf.Sin(angle));
        }

        for (int i = 0; i < nbMeridiens / nbDivise; i++)
        {
            int next = (i + 1) % nbMeridiens;
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = next + 1;
        }

        for (int i = 0; i < nbMeridiens / nbDivise; i++)
        {
            int next = (i + 1) % nbMeridiens;
            triangles[(i + nbMeridiens) * 3] = nbMeridiens + 1;
            triangles[(i + nbMeridiens) * 3 + 1] = nbMeridiens + 2 + i;
            triangles[(i + nbMeridiens) * 3 + 2] = nbMeridiens + 2 + next;
        }

        int t = nbMeridiens * 6;
        for (int i = 0; i < nbMeridiens / nbDivise; i++)
        {
            int next = (i + 1) % nbMeridiens;
            int base1 = i + 1;
            int base2 = next + 1;
            int top1 = nbMeridiens + 2 + i;
            int top2 = nbMeridiens + 2 + next;

            triangles[t] = base1;
            triangles[t + 1] = top1;
            triangles[t + 2] = base2;

            triangles[t + 3] = top1;
            triangles[t + 4] = top2;
            triangles[t + 5] = base2;

            t += 6;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}

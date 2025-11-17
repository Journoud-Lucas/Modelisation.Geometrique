using UnityEngine;

public class SphereCoupe : MonoBehaviour
{
    [SerializeField] private float rayon = 1f;
    [SerializeField] private int nbMerdidiens = 36;
    [SerializeField] private int nbParalleles = 18;
    [SerializeField] private int nbDivise = 1;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[(nbParalleles + 1) * (nbMerdidiens + 1)];
        int[] triangles = new int[nbParalleles * nbMerdidiens * 6];

        int v = 0;
        for (int i = 0; i <= nbParalleles; i++)
        {
            float latitude = Mathf.PI * i / nbParalleles - Mathf.PI / 2;
            for (int j = 0; j <= nbMerdidiens/ nbDivise; j++)
            {
                float longitude = 2 * Mathf.PI * j / nbMerdidiens;
                float x = rayon * Mathf.Cos(latitude) * Mathf.Cos(longitude);
                float y = rayon * Mathf.Sin(latitude);
                float z = rayon * Mathf.Cos(latitude) * Mathf.Sin(longitude);
                vertices[v] = new Vector3(x, y, z);
                v++;
            }
        }

        int t = 0;
        for (int i = 0; i < nbParalleles; i++)
        {
            for (int j = 0; j < nbMerdidiens; j++)
            {
                int current = i * (nbMerdidiens + 1) + j;
                int next = current + nbMerdidiens + 1;

                triangles[t] = current;
                triangles[t + 1] = next;
                triangles[t + 2] = current + 1;

                triangles[t + 3] = next;
                triangles[t + 4] = next + 1;
                triangles[t + 5] = current + 1;

                t += 6;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}

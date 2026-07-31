using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
[ExecuteAlways]
public class TorusGenerator : MonoBehaviour
{
    public float majorRadius = 1f;     // Distance from centre to tube centre
    public float minorRadius = 0.3f;   // Tube thickness

    public int majorSegments = 32;     // Around the donut
    public int minorSegments = 16;     // Around the tube

    void Start()
    {
        GenerateTorus();
    }

    void GenerateTorus()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Torus";

        Vector3[] vertices = new Vector3[(majorSegments + 1) * (minorSegments + 1)];
        int[] triangles = new int[majorSegments * minorSegments * 6];

        int vertex = 0;

        for (int i = 0; i <= majorSegments; i++)
        {
            float u = (float)i / majorSegments * Mathf.PI * 2;

            for (int j = 0; j <= minorSegments; j++)
            {
                float v = (float)j / minorSegments * Mathf.PI * 2;

                float x = (majorRadius + minorRadius * Mathf.Cos(v)) * Mathf.Cos(u);
                float y = minorRadius * Mathf.Sin(v);
                float z = (majorRadius + minorRadius * Mathf.Cos(v)) * Mathf.Sin(u);

                vertices[vertex++] = new Vector3(x, y, z);
            }
        }

        int t = 0;

        for (int i = 0; i < majorSegments; i++)
        {
            for (int j = 0; j < minorSegments; j++)
            {
                int a = i * (minorSegments + 1) + j;
                int b = a + minorSegments + 1;

                triangles[t++] = a;
                triangles[t++] = b;
                triangles[t++] = a + 1;

                triangles[t++] = a + 1;
                triangles[t++] = b;
                triangles[t++] = b + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
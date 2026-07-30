using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject prefab;

    [Header("Layout")]
    public int count = 64;

    [Tooltip("Approximate spacing between neighbouring cubes")]
    public float spacing = 1f;

    [Header("Position")]
    public Vector3 centre = Vector3.zero;

    void Start()
    {
        if (prefab == null)
        {
            Debug.LogError("Assign a prefab.");
            return;
        }

        // Radius needed to achieve the requested spacing.
        float radius = (spacing * count) / (2f * Mathf.PI);

        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;

            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            ) + centre;

            GameObject obj = Instantiate(
                prefab,
                position,
                Quaternion.identity,
                transform
            );

            // Face towards the centre
            obj.transform.LookAt(centre);

            // Turn around so the "front" faces outward if needed.
            // Delete this line if yours already face correctly.
            obj.transform.Rotate(0f, 180f, 0f);
        }
    }
}
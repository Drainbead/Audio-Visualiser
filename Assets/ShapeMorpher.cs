using UnityEngine;

public class ShapeMorpher : MonoBehaviour
{
    [Header("Shapes")]
    public ShapeData[] shapes;


    [Header("Cubes")]
    public GameObject prefab;
    public int count = 64;


    [Header("Morph")]
    public float moveSpeed = 5f;

    public float morphDuration = 5f;

    [Header("Cube Breathing")]
    public float minScaleY = 0.2f;
    public float maxScaleY = 2f;
    public bool scaleWithMorph = true;
    public float breatheSpeed = 1f;


    private Transform[] cubes;

    private int currentShape = 0;
    private float morphAmount = 0f;


    void Start()
    {
        cubes = new Transform[count];


        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(
                prefab,
                shapes[0].positions[i],
                Quaternion.identity,
                transform
            );

            cubes[i] = obj.transform;
        }
    }

    private void morphs()
    {
        if (shapes == null || shapes.Length < 2)
            return;


        int nextShape = currentShape + 1;

        if (nextShape >= shapes.Length)
            nextShape = 0;


        morphAmount += Time.deltaTime / morphDuration;


        if (morphAmount >= 1f)
        {
            morphAmount = 0f;
            currentShape = nextShape;
        }


        for (int i = 0; i < count; i++)
        {
            Vector3 target = Vector3.Lerp(
                shapes[currentShape].positions[i],
                shapes[nextShape].positions[i],
                morphAmount
            );


            cubes[i].position = Vector3.Lerp(
                cubes[i].position,
                target,
                Time.deltaTime * moveSpeed
            );
            if (scaleWithMorph)
            {
                float scaleWave = Mathf.Sin(morphAmount * Mathf.PI);

                float targetScale =
                    Mathf.Lerp(
                        minScaleY,
                        maxScaleY,
                        scaleWave
                    );


                Vector3 scale = cubes[i].localScale;

                scale.y = Mathf.Lerp(
                    scale.y,
                    targetScale,
                    Time.deltaTime * moveSpeed * breatheSpeed
                );

                cubes[i].localScale = scale;
            }
        }
    }
    void Update()
    {
        if (shapes == null || shapes.Length < 2)
            return;


        int nextShape = currentShape + 1;

        if (nextShape >= shapes.Length)
            nextShape = 0;


        morphAmount += Time.deltaTime / morphDuration;


        if (morphAmount >= 1f)
        {
            morphAmount = 0f;
            currentShape = nextShape;
        }


        for (int i = 0; i < count; i++)
        {
            Vector3 target = Vector3.Lerp(
                shapes[currentShape].positions[i],
                shapes[nextShape].positions[i],
                morphAmount
            );


            cubes[i].localPosition = Vector3.Lerp(
                cubes[i].localPosition,
                target,
                Time.deltaTime * moveSpeed
            );
            if (scaleWithMorph)
            {
                float scaleWave = Mathf.Sin(morphAmount * Mathf.PI);

                float targetScale =
                    Mathf.Lerp(
                        minScaleY,
                        maxScaleY,
                        scaleWave
                    );


                Vector3 scale = cubes[i].localScale;

                scale.y = Mathf.Lerp(
                    scale.y,
                    targetScale,
                    Time.deltaTime * moveSpeed * breatheSpeed
                );

                cubes[i].localScale = scale;
            }
        }
    }
}
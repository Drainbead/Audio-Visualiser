using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    public float height = 5f;
    public float wavyHeight = 2f;
    public float wavyFrequency = 0.25f;
    public SavedShapes savedShapes;
    public string shapeName = "Circle";
    private List<Vector3> generatedPositions = new List<Vector3>();
    public int waveCount = 16;
    public enum Shape
    {
        Circle,
        DoubleCircle,
        Spiral,
        Helix,
        DoubleHelix,
        Heart,
        Sphere,
        Star,
        LetterA,
        Line,
        CoilSnake,
        CoilCircle,
        WavyLine,
        Grid
    }

    [Header("General")]
    public Shape shape = Shape.Circle;
    public GameObject prefab;

    public int count = 64;
    public float spacing = 1f;

    [Header("Circle")]
    public float radius = 10f;

    [Header("Coil Snake")]
    public float coilSpacing = 1f;
    public int coilStepLength = 3;
    [Header("Snake Wave")]
    public float waveHeight = 2f;
    public float waveFrequency = 0.15f;

    [Header("Circle Coil")]
    public float CirclecoilcoilSpacing = 0.5f;
    public float coilTurnSpeed = 0.25f;

    [Header("Line")]
    public float lineSpacing = 1f;
    public bool verticalLine = false;

    [Header("Spiral")]
    public float spiralGrowth = 0.15f;

    [Header("Helix")]
    public float helixHeight = 8f;
    public float helixTurns = 3f;

    [Header("Grid")]
    public int columns = 8;

    [Header("Shape Scale")]
    public Vector3 shapeScale = Vector3.one;

    void Start()
    {
        Generate();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveCurrentShape();
        }
    }
    [ContextMenu("Generate")]
    public void Generate()
    {
        if (prefab == null)
            return;
        generatedPositions.Clear();
        // Delete old children
        while (transform.childCount > 0)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(0).gameObject);
#else
            Destroy(transform.GetChild(0).gameObject);
#endif
        }

        switch (shape)
        {
            case Shape.Circle:
                GenerateCircle(radius);
                break;

            case Shape.DoubleCircle:
                GenerateDoubleCircle();
                break;

            case Shape.Spiral:
                GenerateSpiral();
                break;

            case Shape.Helix:
                GenerateHelix();
                break;

            case Shape.Grid:
                GenerateGrid();
                break;

            case Shape.DoubleHelix:
                GenerateDoubleHelix();
                break;

            case Shape.Heart:
                GenerateHeart();
                break;

            case Shape.Sphere:
                GenerateSphere();
                break;

            case Shape.Star:
                GenerateStar();
                break;

            case Shape.LetterA:
                GenerateLetterA();
                break;

            case Shape.Line:
                GenerateLine();
                break;

            case Shape.CoilSnake:
                GenerateCoilSnake();
                break;

            case Shape.CoilCircle:
                GenerateCoilCircle();
                break;

            case Shape.WavyLine:
                GenerateWavyLine();
                break;
        }
       
    }

    void GenerateCircle(float r)
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;

            Vector3 pos = new Vector3(
                Mathf.Cos(angle),
                height,
                Mathf.Sin(angle)
            ) * r;

            Spawn(pos);
        }
    }
    void GenerateCoilSnake()
    {
        Vector3 pos = Vector3.zero;

        Vector3[] directions =
        {
        Vector3.forward,
        Vector3.left,
        Vector3.back,
        Vector3.right
    };

        int directionIndex = 0;

        int stepsTaken = 0;
        int stepsNeeded = coilStepLength;

        int lengthIncrease = 0;


        for (int i = 0; i < count; i++)
        {
            Spawn(new Vector3(
     pos.x,
     height + Mathf.Sin(i * waveFrequency) * waveHeight,
     pos.z
 ));


            pos += directions[directionIndex] * coilSpacing;


            stepsTaken++;


            if (stepsTaken >= stepsNeeded)
            {
                stepsTaken = 0;

                directionIndex++;

                if (directionIndex >= directions.Length)
                {
                    directionIndex = 0;
                }


                lengthIncrease++;

                if (lengthIncrease % 2 == 0)
                {
                    stepsNeeded++;
                }
            }
        }
    }
    void GenerateCoilCircle()
    {
        float angle = 0f;

        for (int i = 0; i < count; i++)
        {
            float radius = CirclecoilcoilSpacing * angle / (Mathf.PI * 2f);

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                height,
                Mathf.Sin(angle) * radius
            );

            Spawn(pos);

            angle += coilTurnSpeed;
        }
    }
    void GenerateLine()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 pos;


            if (verticalLine)
            {
                pos = new Vector3(
                    0,
                    (i - count / 2f) * lineSpacing,
                    0
                );
            }
            else
            {
                pos = new Vector3(
                    (i - count / 2f) * lineSpacing,
                    height,
                    0
                );
            }


            Spawn(pos);
        }
    }
    void GenerateWavyLine()
    {
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / (count - 1);

            float offset =
                Mathf.Sin(t * Mathf.PI * waveCount) * wavyHeight; ;

            Vector3 pos;

            if (verticalLine)
            {
                pos = new Vector3(
                    offset,
                    (i - count / 2f) * lineSpacing,
                    0
                );
            }
            else
            {
                pos = new Vector3(
                    (i - count / 2f) * lineSpacing,
                    height + offset,
                    0
                );
            }

            Spawn(pos);
        }
    }
    void GenerateLetterA()
    {
        Vector3[] points =
        {
        new Vector3(-1,2,0),
        new Vector3(0,3,0),
        new Vector3(1,2,0),

        new Vector3(-1,1,0),
        new Vector3(1,1,0),

        new Vector3(-1,0,0),
        new Vector3(0,0,0),
        new Vector3(1,0,0),

        new Vector3(-1,-1,0),
        new Vector3(1,-1,0)
    };


        foreach (Vector3 p in points)
        {
            Spawn(p * spacing + Vector3.up * height);
        }
    }
    void GenerateStar()
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2f / count;

            float r = (i % 2 == 0)
                ? radius
                : radius * 0.4f;


            Spawn(new Vector3(
                Mathf.Cos(angle) * r,
                height,
                Mathf.Sin(angle) * r
            ));
        }
    }
    void GenerateSphere()
    {
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / count;

            float angle = t * Mathf.PI * 2f * 4f;

            float y = Mathf.Lerp(
                -radius,
                radius,
                t
            );


            float circleRadius =
                Mathf.Sqrt(
                    radius * radius - y * y
                );


            Spawn(new Vector3(
                Mathf.Cos(angle) * circleRadius,
                y + height,
                Mathf.Sin(angle) * circleRadius
            ));
        }
    }
    void GenerateDoubleHelix()
    {
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / count;

            float angle = helixTurns * Mathf.PI * 2f * t;

            float y = t * helixHeight;


            // First strand
            Spawn(new Vector3(
                Mathf.Cos(angle) * radius,
                y,
                Mathf.Sin(angle) * radius
            ));


            // Second strand (180 degrees opposite)
            Spawn(new Vector3(
                Mathf.Cos(angle + Mathf.PI) * radius,
                y,
                Mathf.Sin(angle + Mathf.PI) * radius
            ));
        }
    }
    void GenerateHeart()
    {
        for (int i = 0; i < count; i++)
        {
            float t = Mathf.PI * 2f * i / count;


            float x = 16f * Mathf.Pow(Mathf.Sin(t), 3);


            float y =
                13f * Mathf.Cos(t)
                - 5f * Mathf.Cos(2f * t)
                - 2f * Mathf.Cos(3f * t)
                - Mathf.Cos(4f * t);


            Spawn(new Vector3(
                x * 0.1f,
                y * 0.1f + height,
                0
            ));
        }
    }
    void GenerateDoubleCircle()
    {
        int inner = count / 2;
        int outer = count - inner;

        for (int i = 0; i < inner; i++)
        {
            float angle = i * Mathf.PI * 2f / inner;

            Spawn(new Vector3(
                Mathf.Cos(angle),
                height,
                Mathf.Sin(angle)
            ) * radius);
        }

        for (int i = 0; i < outer; i++)
        {
            float angle = i * Mathf.PI * 2f / outer;

            Spawn(new Vector3(
                Mathf.Cos(angle),
                height,
                Mathf.Sin(angle)
            ) * (radius + spacing * 2f));
        }
    }

    void GenerateSpiral()
    {
        for (int i = 0; i < count; i++)
        {
            float angle = i * 0.35f;

            float r = radius + i * spiralGrowth;

            Spawn(new Vector3(
                Mathf.Cos(angle) * r,
                height,
                Mathf.Sin(angle) * r
            ));
        }
    }

    void GenerateHelix()
    {
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / count;

            float angle = helixTurns * Mathf.PI * 2f * t;

            Spawn(new Vector3(
                Mathf.Cos(angle) * radius,
                t * helixHeight,
                Mathf.Sin(angle) * radius
            ));
        }
    }

    void GenerateGrid()
    {
        for (int i = 0; i < count; i++)
        {
            int x = i % columns;
            int z = i / columns;

            Spawn(new Vector3(
                x * spacing,
                height,
                z * spacing
            ));
        }
    }

    void Spawn(Vector3 position)
    {
        position = Vector3.Scale(
            position,
            shapeScale
        );

        generatedPositions.Add(position);

        GameObject obj = Instantiate(
            prefab,
            position,
            Quaternion.identity,
            transform
        );
    }
    public void SaveCurrentShape()
    {
        if (savedShapes == null)
        {
            Debug.LogError("No SavedShapes assigned.");
            return;
        }

        if (generatedPositions.Count == 0)
        {
            Debug.LogError("No generated positions to save.");
            return;
        }

        savedShapes.SaveShape(
            shapeName,
            generatedPositions.ToArray()
        );

        Debug.Log("Saved " + shapeName + " (" + generatedPositions.Count + " positions)");
    }
}
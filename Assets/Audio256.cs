using UnityEngine;
using UnityEngine.Audio;


public class Audio256 : MonoBehaviour
{
    [Header("Scene References")]
    public Transform pivot;
    public Transform[] bars;
    public bool autoFindBars = true;

    [Header("Emission")]
    public Color bassColor = new Color(1f, 0.1f, 0.5f);
    public Color midColor = new Color(0.1f, 0.5f, 1f);
    public Color highColor = new Color(0.2f, 1f, 1f);

    public float emissionStrength = 5f;

    private MaterialPropertyBlock propertyBlock;

    [Header("Visual")]
    public float minHeight = 0.2f;
    public float maxHeight = 20f;
    public float sensitivity = 500f;
    public float fallSpeed = 10f;
    [Header("Bass")]
    public float bassReduction = 0.45f;
    public float compression = 0.5f;

    [Header("Orbit")]
    public bool orbit = true;
    public float baseOrbitSpeed = 15f;
    public float orbitMultiplier = 120f;
    public float orbitSmooth = 3f;

    [Header("Orbit Breathing")]
    public float radiusMultiplier = 3f;
    public float radiusSmooth = 5f;
    public float frequencyRadiusBoost = 0.15f;

    [Header("Bar Rotation")]
    public bool rotateBars = true;
    public float baseRotationSpeed = 20f;
    public float rotationMultiplier = 500f;
    public float rotationAcceleration = 5f;

    private float[] rotationVelocities;

    public AudioManager audioManager;

    private float[] bands;

    private float[] orbitAngles;
    private float[] orbitSpeeds;
    private float[] orbitRadii;
    private float[] currentRadii;
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


    

    private int currentShape = 0;
    private float morphAmount = 0f;

    void Start()
    {
        if (autoFindBars)
        {
            bars = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                bars[i] = transform.GetChild(i);
            }
        }

        Debug.Log("Found " + bars.Length + " bars");
        propertyBlock = new MaterialPropertyBlock();


        int count = bars.Length;

        orbitAngles = new float[count];
        orbitSpeeds = new float[count];
        orbitRadii = new float[count];
        currentRadii = new float[count];

        rotationVelocities = new float[bars.Length];

        for (int i = 0; i < bars.Length; i++)
        {
            rotationVelocities[i] = baseRotationSpeed;
        }


        for (int i = 0; i < count; i++)
        {
            Vector3 offset = bars[i].position - pivot.position;

            orbitRadii[i] = new Vector2(
                offset.x,
                offset.z
            ).magnitude;

            currentRadii[i] = orbitRadii[i];

            orbitAngles[i] = Mathf.Atan2(
                offset.z,
                offset.x
            ) * Mathf.Rad2Deg;

            orbitSpeeds[i] = baseOrbitSpeed;
        }
    }

    void MorphShapes()
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


        for (int i = 0; i < bars.Length; i++)
        {
            Vector3 localTarget = Vector3.Lerp(
                shapes[currentShape].positions[i],
                shapes[nextShape].positions[i],
                morphAmount
            );


            bars[i].localPosition = Vector3.Lerp(
                bars[i].localPosition,
                localTarget,
                Time.deltaTime * moveSpeed
            );
        }
    }
    void Update()
    {
        bands = audioManager.GetBands(bars.Length);


        int numberOfBars = bars.Length;


        for (int i = 0; i < numberOfBars; i++)
        {



            float highest = bands[i];


            // Compression
            highest = Mathf.Pow(
                highest,
                compression
            );


            // Reduce bass dominance
            highest *= Mathf.Lerp(
                bassReduction,
                1f,
                (float)i / bars.Length
            );


            // HEIGHT
            float targetHeight = Mathf.Clamp(
                highest * sensitivity,
                minHeight,
                maxHeight
            );


            Vector3 scale = bars[i].localScale;


            if (targetHeight > scale.y)
            {
                scale.y = targetHeight;
            }
            else
            {
                scale.y = Mathf.Lerp(
                    scale.y,
                    targetHeight,
                    Time.deltaTime * fallSpeed
                );
            }


            bars[i].localScale = scale;
            // Emission glow based on height
            // Frequency based colour
float frequencyPosition = (float)i / (numberOfBars - 1);

Color frequencyColor;

if (frequencyPosition < 0.33f)
{
    // Bass
    frequencyColor = Color.Lerp(
        bassColor,
        midColor,
        frequencyPosition / 0.33f
    );
}
else if (frequencyPosition < 0.66f)
{
    // Mid
    frequencyColor = Color.Lerp(
        midColor,
        highColor,
        (frequencyPosition - 0.33f) / 0.33f
    );
}
else
{
    // High
    frequencyColor = highColor;
}


// Brightness from volume
float glow = Mathf.InverseLerp(
    minHeight,
    maxHeight,
    scale.y
);


Color emission =
    frequencyColor *
    glow *
    emissionStrength;

            // Smooth X axis rotation
            if (rotateBars)
            {
                float targetVelocity =
                    baseRotationSpeed +
                    highest * rotationMultiplier;

                rotationVelocities[i] = Mathf.Lerp(
                    rotationVelocities[i],
                    targetVelocity,
                    Time.deltaTime * rotationAcceleration
                );


                bars[i].Rotate(
                    Vector3.right,
                    rotationVelocities[i] * Time.deltaTime,
                    Space.Self
                );
            }


            Renderer renderer = bars[i].GetComponent<Renderer>();

renderer.GetPropertyBlock(propertyBlock);

propertyBlock.SetColor(
    "_EmissionColor",
    emission
);

renderer.SetPropertyBlock(propertyBlock);


            // ORBIT
            if (orbit)
            {
                // Rotation speed
                float targetSpeed =
                    baseOrbitSpeed +
                    highest * orbitMultiplier;


                orbitSpeeds[i] = Mathf.Lerp(
                    orbitSpeeds[i],
                    targetSpeed,
                    Time.deltaTime * orbitSmooth
                );


                orbitAngles[i] +=
                    orbitSpeeds[i] * Time.deltaTime;


                // Breathing radius
                float targetRadius =
                    orbitRadii[i] +
                    highest *
                    radiusMultiplier *
                    (1f + i * frequencyRadiusBoost);


                currentRadii[i] = Mathf.Lerp(
                    currentRadii[i],
                    targetRadius,
                    Time.deltaTime * radiusSmooth
                );


                float radians =
                    orbitAngles[i] * Mathf.Deg2Rad;


                Vector3 pos = new Vector3(
                    Mathf.Cos(radians) * currentRadii[i],
                    scale.y * 0.5f,
                    Mathf.Sin(radians) * currentRadii[i]
                );


                bars[i].position =
                    pivot.position + pos;


                // Face away from centre
                bars[i].LookAt(pivot.position);
                bars[i].Rotate(0f, 180f, 0f);
            }
        }
        MorphShapes();
    }
}
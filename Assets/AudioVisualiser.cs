using UnityEngine;
using UnityEngine.Audio;
//https://www.youtube.com/watch?v=7OjqeyOvC1c woodwind instruments THIS IS BEST and https://www.youtube.com/watch?v=v62YjjV-Roo
//https://www.youtube.com/watch?v=IwZ8Q9VvknY 50 bird calls AWESOME
      // https://www.youtube.com/watch?v=8wWHEE4A9xQ
      //https://music.youtube.com/watch?v=lPUk7AQuoLM&list=LM
      //https://www.youtube.com/watch?v=FBglnc2D9y4 // adele vocals accapella
//https://www.youtube.com/watch?v=qISirGv3Mho creep accapella by a woman
      // https://music.youtube.com/watch?v=dZ3IHSLpV_c&list=LM
      // https://music.youtube.com/watch?v=59Q_lhgGANc&list=LM
//https://music.youtube.com/watch?v=Gn6SNbTD0_Q&list=LM jackie wilsome - higher and higher
//https://www.youtube.com/watch?v=AnUt3eRGIeE&list=PLDDC74011619A10AE&index=25 ocarina of time
//https://www.youtube.com/watch?v=yfrhek3O8ew&list=PLDDC74011619A10AE&index=31 ocarina some level

public class AudioVisualiser : MonoBehaviour
{
    [Header("Scene References")]
    public Transform pivot;
    public Transform[] bars;

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

    [Header("Frequency")]
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

    public AudioSource audioSource;


    private float[] spectrum = new float[512];

    private float[] orbitAngles;
    private float[] orbitSpeeds;
    private float[] orbitRadii;
    private float[] currentRadii;


    void Start()
    {

        propertyBlock = new MaterialPropertyBlock();


        int count = bars.Length;

        orbitAngles = new float[count];
        orbitSpeeds = new float[count];
        orbitRadii = new float[count];
        currentRadii = new float[count];


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


    void Update()
    {
        audioSource.GetSpectrumData(
        spectrum,
        0,
        FFTWindow.BlackmanHarris
 );


        int numberOfBars = bars.Length;


        for (int i = 0; i < numberOfBars; i++)
        {
            // Logarithmic frequency band
            int start = Mathf.FloorToInt(
                Mathf.Pow(512f, (float)i / numberOfBars)
            );

            int end = Mathf.FloorToInt(
                Mathf.Pow(512f, (float)(i + 1) / numberOfBars)
            );


            start = Mathf.Clamp(start, 1, 511);
            end = Mathf.Clamp(end, start + 1, 512);


            float highest = 0f;


            for (int j = start; j < end; j++)
            {
                if (spectrum[j] > highest)
                    highest = spectrum[j];
            }


            // Compression
            highest = Mathf.Pow(
                highest,
                compression
            );


            // Reduce bass dominance
            highest *= Mathf.Lerp(
                bassReduction,
                1f,
                (float)i / numberOfBars
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
    }
}
using UnityEngine;

public class BeatCube : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Beat Detection")]
    public float sensitivity = 5000f;
    public float beatMultiplier = 1.35f;
    public float averageSpeed = 2f;
    public float cooldownTime = 0.15f;

    [Header("Visual")]
    public float normalSize = 1f;
    public float beatSize = 2f;
    public float growSpeed = 15f;
    public float shrinkSpeed = 5f;

    [Header("Emission")]
    public Renderer cubeRenderer;
    public Color normalColor = Color.cyan;
    public Color beatColor = Color.white;
    public float emissionStrength = 5f;

    private float[] spectrum = new float[512];

    private float bassAverage = 0f;
    private float beatCooldown = 0f;

    private Vector3 targetScale;

    private MaterialPropertyBlock block;


    void Start()
    {
        targetScale = Vector3.one * normalSize;

        block = new MaterialPropertyBlock();

        SetGlow(normalColor);
    }


    void Update()
    {
        audioSource.GetSpectrumData(
            spectrum,
            0,
            FFTWindow.BlackmanHarris
        );


        // Get bass energy
        float bass = 0f;

        for (int i = 1; i < 30; i++)
        {
            bass += spectrum[i];
        }

        bass /= 30f;

        bass *= sensitivity;


        // Build rolling average
        bassAverage = Mathf.Lerp(
            bassAverage,
            bass,
            Time.deltaTime * averageSpeed
        );


        if (beatCooldown > 0)
        {
            beatCooldown -= Time.deltaTime;
        }


        // Beat detected
        if (bass > bassAverage * beatMultiplier &&
            beatCooldown <= 0)
        {
            targetScale = Vector3.one * beatSize;

            SetGlow(beatColor);

            beatCooldown = cooldownTime;
        }


        // Return to normal size
        targetScale = Vector3.Lerp(
            targetScale,
            Vector3.one * normalSize,
            Time.deltaTime * shrinkSpeed
        );


        float speed = targetScale.x > transform.localScale.x
            ? growSpeed
            : shrinkSpeed;


        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * speed
        );


        // Return colour after beat
        SetGlow(Color.Lerp(
            GetCurrentGlow(),
            normalColor,
            Time.deltaTime * 5f
        ));
    }


    void SetGlow(Color colour)
    {
        if (cubeRenderer == null)
            return;

        cubeRenderer.GetPropertyBlock(block);

        block.SetColor(
            "_EmissionColor",
            colour * emissionStrength
        );

        cubeRenderer.SetPropertyBlock(block);
    }


    Color GetCurrentGlow()
    {
        return normalColor;
    }
}
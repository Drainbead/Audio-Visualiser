using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;


    [Header("Auto Gain")]
    public float gainSpeed = 2f;
    public float targetLevel = 0.05f;
    private int beatCount = 0;
    private float currentGain = 1f;


    [Header("Beat Detection")]
    public float beatSensitivity = 1.5f;
    public float beatCooldown = 0.25f;
    public float minimumBass = 0.05f;
    private float previousBass;
    public float beatThreshold = 0.01f;

    public float BeatPulse { get; private set; }

    [Header("Debug Beat Cube")]
    public Transform beatCube;
    public Vector3 beatScale = new Vector3(2f, 2f, 2f);
    public float beatReturnSpeed = 8f;

    private Vector3 beatOriginalScale;
    private Vector3 beatCurrentScale;

    public bool Beat { get; private set; }
    public float Bass { get; private set; }
    public float Mid { get; private set; }
    public float High { get; private set; }


    private float bassAverage;
    private float lastBeatTime;
    public string audioDeviceContains = "CABLE Output";

    private string currentDevice;

    private float[] spectrum = new float[512];
    void Start()
    {
        if (beatCube != null)
        {
            beatOriginalScale = beatCube.localScale;
            beatCurrentScale = beatOriginalScale;
        }
        audioSource = GetComponent<AudioSource>();

        StartCapture();
    }
    void StartCapture()
    {
        foreach (string device in Microphone.devices)
        {
            Debug.Log("MIC DEVICE: " + device);
        }


        currentDevice = FindDevice(audioDeviceContains);


        if (string.IsNullOrEmpty(currentDevice))
        {
            Debug.LogError("Audio device not found");
            return;
        }


        Debug.Log("Using audio device: " + currentDevice);


        audioSource.clip = Microphone.Start(
            currentDevice,
            true,
            1,
            48000
        );


        audioSource.loop = true;


        while (Microphone.GetPosition(currentDevice) <= 0)
        {
        }


        audioSource.Play();
    }
    string FindDevice(string contains)
    {
        foreach (string device in Microphone.devices)
        {
            if (device.ToLower().Contains(contains.ToLower()))
                return device;
        }

        return null;
    }
    void Update()
    {
        audioSource.GetSpectrumData(
            spectrum,
            0,
            FFTWindow.BlackmanHarris
        );


        CalculateBands();

        DetectBeat();
        UpdateBeatCube();
    }

    void UpdateBeatCube()
    {
        if (beatCube == null)
            return;

        if (Beat)
        {
            beatCurrentScale = beatScale;
        }

        beatCurrentScale = Vector3.Lerp(
            beatCurrentScale,
            beatOriginalScale,
            Time.deltaTime * beatReturnSpeed
        );

        beatCube.localScale = beatCurrentScale;
    }

    public float[] GetBands(int bandCount)
    {
        float[] bands = new float[bandCount];


        for (int i = 0; i < bandCount; i++)
        {
            int start = Mathf.FloorToInt(
                Mathf.Pow(512f, (float)i / bandCount)
            );

            int end = Mathf.FloorToInt(
                Mathf.Pow(512f, (float)(i + 1) / bandCount)
            );


            start = Mathf.Clamp(start, 1, 511);
            end = Mathf.Clamp(end, start + 1, 512);


            float highest = 0f;


            for (int j = start; j < end; j++)
            {
                if (spectrum[j] > highest)
                    highest = spectrum[j];
            }


            highest *= currentGain;

            bands[i] = Mathf.Clamp01(highest);
        }


        return bands;
    }



    void CalculateBands()
    {
        float averageEnergy = 0f;


        for (int i = 1; i < 512; i++)
        {
            averageEnergy += spectrum[i];
        }


        averageEnergy /= 511f;


        float gainTarget = targetLevel /
            Mathf.Max(averageEnergy, 0.001f);


        currentGain = Mathf.Lerp(
            currentGain,
            gainTarget,
            Time.deltaTime * gainSpeed
        );


        Bass = GetFrequency(2, 12) * currentGain;
        Mid = GetFrequency(20, 150) * currentGain;
        High = GetFrequency(150, 400) * currentGain;
    }



    float GetFrequency(int start, int end)
    {
        float highest = 0f;


        for (int i = start; i < end; i++)
        {
            if (spectrum[i] > highest)
                highest = spectrum[i];
        }


        return highest;
    }



    void DetectBeat()
    {
        Beat = false;


        float bassChange = Bass - previousBass;

        previousBass = Bass;


        bassAverage = Mathf.Lerp(
            bassAverage,
            Bass,
            Time.deltaTime * 0.5f
        );


        if (Time.time - lastBeatTime < beatCooldown)
            return;


        if (
            bassChange > beatThreshold &&
            Bass > minimumBass
        )
        {
            Beat = true;

            lastBeatTime = Time.time;

            beatCount++;

            Debug.Log(
                "BEAT " + beatCount +
                " Bass:" + Bass +
                " Change:" + bassChange +
                " Average:" + bassAverage
            );
        }
    }
}
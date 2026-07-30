using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Auto Gain")]
    public float gainSpeed = 2f;
    public float targetLevel = 0.05f;

    private float currentGain = 1f;

    private float[] spectrum = new float[512];


    public float[] GetBands(int bandCount)
    {
        float[] bands = new float[bandCount];

        audioSource.GetSpectrumData(
            spectrum,
            0,
            FFTWindow.BlackmanHarris
        );


        // Find overall audio energy
        float averageEnergy = 0f;

        for (int i = 1; i < 512; i++)
        {
            averageEnergy += spectrum[i];
        }

        averageEnergy /= 511f;


        // Calculate automatic gain
        float gainTarget = targetLevel / Mathf.Max(
            averageEnergy,
            0.001f
        );


        currentGain = Mathf.Lerp(
            currentGain,
            gainTarget,
            Time.deltaTime * gainSpeed
        );


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


            // Apply automatic volume correction
            highest *= currentGain;


            // Keep values sensible
            highest = Mathf.Clamp01(highest);


            bands[i] = highest;
        }


        return bands;
    }
}
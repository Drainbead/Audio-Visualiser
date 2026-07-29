using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AudioSource))]
public class DesktopAudioCapture : MonoBehaviour
{
    [Header("Device Search")]
    public string desktopDeviceContains = "CABLE Output";
    public string microphoneDeviceContains = "C-Media";

    [Header("Current Mode")]
    public bool useDesktopAudio = true;

    private AudioSource source;
    private string currentDevice;

    void Start()
    {
        source = GetComponent<AudioSource>();

        foreach (string device in Microphone.devices)
        {
            Debug.Log("MIC DEVICE: " + device);
        }

        StartCapture();
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            useDesktopAudio = true;
            StartCapture();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            useDesktopAudio = false;
            StartCapture();
        }
    }

    void StartCapture()
    {
        // Stop previous capture
        if (!string.IsNullOrEmpty(currentDevice))
            Microphone.End(currentDevice);

        currentDevice = FindDevice(
            useDesktopAudio ?
            desktopDeviceContains :
            microphoneDeviceContains);

        if (string.IsNullOrEmpty(currentDevice))
        {
            Debug.LogError("Couldn't find requested device.");
            return;
        }

        Debug.Log("Using: " + currentDevice);

        source.Stop();

        source.clip = Microphone.Start(
            currentDevice,
            true,
            1,
            48000
        );

        source.loop = true;
        source.volume = 0.011f;

        while (Microphone.GetPosition(currentDevice) <= 0)
        {
        }

        source.Play();
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

    void OnDisable()
    {
        if (!string.IsNullOrEmpty(currentDevice))
            Microphone.End(currentDevice);
    }
}
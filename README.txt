Unity Audio Visualiser

A real-time Unity audio visualiser that reacts to:

Desktop audio
Microphone input
Music
Vocals
Nature sounds
Features

✓ Logarithmic frequency bands
✓ Bloom glow effects
✓ Dynamic emission colours
✓ Desktop audio capture
✓ Microphone mode
✓ Live switching between audio sources
✓ Real-time frequency visualisation

IMPORTANT - Setup Instructions
Requirements

Install:

Unity 6.3+
VB-Audio Virtual Cable
Voicemeeter
Desktop Audio Setup
1. Install audio software

Install:

VB-CABLE
Voicemeeter

Restart Windows after installation.

2. Configure Voicemeeter

Open Voicemeeter.

Go to the hardware output section.

Set:

A1:

CABLE Input (VB-Audio Virtual Cable)

A2:
Set this to your normal audio output.

Example:

Speakers (Realtek(R) Audio)

This allows Unity to receive the audio while you can still hear it through your normal speakers/headphones.

3. Configure Windows Sound Settings

Open:

Windows Sound Settings

Set:

Output device
Voicemeeter Input
Input device
CABLE Output (VB-Audio Virtual Cable)
4. Check Recording Devices

Open:

Sound Settings
→ More sound settings
→ Recording

Make sure:

CABLE Output (VB-Audio Virtual Cable)

is enabled.

Unity reads these devices as microphone inputs.

Finding Your Microphone / Audio Device Name

When Unity starts, the DesktopAudioCapture script prints available devices to the Console.

Example:

MIC DEVICE: CABLE Output (VB-Audio Virtual Cable)

MIC DEVICE: Microphone (C-Media USB Audio Device)

Copy the exact device name you want to use.

In Unity:

DesktopAudioCapture script
→ Device Name

Paste the device name into the variable in the Inspector.

This allows you to choose between:

Desktop audio through VB-CABLE
A real microphone
Other audio input devices
Controls
F1 = Desktop audio mode

F2 = Microphone mode
Running The Project
Open the project in Unity 6.3+
Configure your audio device
Press Play

The visualiser should now react to your chosen audio source.

Troubleshooting
No animation

Check:

The correct audio device name is entered.
VB-CABLE is selected correctly.
Windows output is set to Voicemeeter Input.
The Unity Console shows the expected microphone device.
Wrong microphone

Look in the Unity Console for the exact device names and copy the one you want into the DesktopAudioCapture Inspector field.

Future Ideas
Beat detection
Particle effects
Audio-reactive creatures
Fish and organic shapes
Fog and lighting effects
More advanced audio interactions

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

Try The Standalone Version (No Unity Required)

A standalone build is included in this repository.

Open the folder:

Game

Run:

survive tactical hunting.exe

You do not need Unity installed to try the visualiser.

However, you still need:

VB-Audio Virtual Cable
Voicemeeter

You will also need to configure Windows audio settings correctly for desktop audio capture.

See the setup instructions below.

Unity Project Setup

If you want to edit or develop the project:

Requirements:

Unity 6.3+

Clone the project and open it in Unity.

Audio Setup
Install Required Software

Install:

VB-Audio Virtual Cable
Voicemeeter

Restart Windows after installation.

Configure Voicemeeter

Open Voicemeeter.

Find the hardware output section.

Set:

A1
CABLE Input (VB-Audio Virtual Cable)
A2

Set this to your normal audio output.

Example:

Speakers (Realtek(R) Audio)

This allows Unity to receive the audio while you can still hear it through your speakers/headphones.

Configure Windows Sound Settings

Open:

Windows Sound Settings

Set:

Output device
Voicemeeter Input
Input device
CABLE Output (VB-Audio Virtual Cable)
Check Recording Devices

Open:

Sound Settings
→ More sound settings
→ Recording

Make sure:

CABLE Output (VB-Audio Virtual Cable)

is enabled.

Unity detects these devices as microphone inputs.

Finding Your Audio Device Name

When the project starts, the DesktopAudioCapture script lists available devices in the Unity Console.

Example:

MIC DEVICE: CABLE Output (VB-Audio Virtual Cable)

MIC DEVICE: Microphone (C-Media USB Audio Device)

Copy the exact device name you want to use.

In Unity:

DesktopAudioCapture script
→ Device Name

Paste the device name into the Inspector.

This allows you to select:

Desktop audio through VB-CABLE
Your microphone
Other connected audio devices
Controls
F1 = Desktop Audio mode

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
VB-CABLE is installed correctly.
Windows output is set to Voicemeeter Input.
The Unity Console shows the expected audio device.
Wrong microphone

Look at the Unity Console device list and copy the exact name into the DesktopAudioCapture Inspector field.

Standalone build does not start

Make sure you are running:

survive tactical hunting.exe

from inside the complete Game folder.

Do not move the .exe by itself because Unity builds require the accompanying _Data folder.

Controls / Future Ideas

Planned ideas:

Beat detection
Particle effects
Audio-reactive creatures
Fish and organic shapes
Fog and lighting effects
More advanced audio interactions

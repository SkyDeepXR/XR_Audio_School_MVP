using UnityEngine;
using UnityEngine.Audio;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;

[System.Serializable]
public class Fader
{
    public Transform faderTransform;
    public float minY;
    public float maxY;
}

public class VolumeFaderMixer : MonoBehaviour
{
    [Header("Channel Volume Faders")]
    public Fader[] volumeFaders;  // Assumes faders move along the Y-axis

    [Header("Master Volume Faders")]
    public Fader masterFaderLeft;
    public Fader masterFaderRight;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;
    
    [Header("Mixer Group Volume Parameters")]
    public string[] volumeParameters; // Names of exposed volume parameters

    private int channelCount;

    void Start()
    {
        channelCount = volumeFaders.Length;
    }

    void Update()
    {
        for (int i = 0; i < channelCount; i++)
        {
            UpdateVolumeFader(volumeFaders[i], volumeParameters[i]);
        }
        UpdateMasterVolumeFader(masterFaderLeft, "MasterVolumeLeft");
        UpdateMasterVolumeFader(masterFaderRight, "MasterVolumeRight");
    }

    void UpdateVolumeFader(Fader fader, string parameterName)
    {
        float faderValue = Mathf.InverseLerp(fader.minY, fader.maxY, fader.faderTransform.localPosition.y);
        SetVolume(parameterName, faderValue);
    }

    void UpdateMasterVolumeFader(Fader fader, string paramName)
    {
        float faderValue = Mathf.InverseLerp(fader.minY, fader.maxY, fader.faderTransform.localPosition.y);
        SetVolume(paramName, faderValue);
    }

    void SetVolume(string parameterName, float value)
    {
        float dbValue = Mathf.Lerp(-80f, 0f, value); // Convert to decibels for the AudioMixer
        audioMixer.SetFloat(parameterName, dbValue);
        Debug.Log($"Set volume for {parameterName} to {dbValue} dB");
    }
}
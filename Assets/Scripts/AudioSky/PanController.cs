using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotaryKnob
{
    public Transform knobTransform;
    public float minRotation;
    public float maxRotation;
    
}

[System.Serializable]
public class PanAudioSource
{
    public Transform audioSourceTransform;
    public float minXPosition;
    public float maxXPosition;
}

public class PanController : MonoBehaviour
{
    [Header("Rotary Knobs for Panning")]
    public RotaryKnob[] panKnobs;

    [Header("Audio Sources for Panning")]
    public PanAudioSource[] audioSources;

    private int channelCount;

    void Start()
    {
        channelCount = panKnobs.Length;
    }

    void Update()
    {
        for (int i = 0; i < channelCount; i++)
        {
            UpdatePan(panKnobs[i], audioSources[i]);
        }
    }

    void UpdatePan(RotaryKnob knob, PanAudioSource audioSource)
    {
        float rotationValue = Mathf.InverseLerp(knob.minRotation, knob.maxRotation, knob.knobTransform.localEulerAngles.z);
        float xPos = Mathf.Lerp(audioSource.minXPosition, audioSource.maxXPosition, rotationValue);
        xPos = Mathf.Clamp(xPos, audioSource.minXPosition, audioSource.maxXPosition); // Clamp the position

        audioSource.audioSourceTransform.localPosition = new Vector3(xPos, audioSource.audioSourceTransform.localPosition.y, audioSource.audioSourceTransform.localPosition.z);
        Debug.Log($"Pan {audioSource.audioSourceTransform.name} to X position {xPos}");
    }
}

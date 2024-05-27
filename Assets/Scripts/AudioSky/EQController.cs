using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class EQKnob
{
    public Transform knobTransform;
    public float minRotation;
    public float maxRotation;
    public string parameterName;  // Name of the exposed EQ parameter
}

public class EQController : MonoBehaviour
{
    [Header("Rotary Knobs for EQ Control")]
    public EQKnob[] eqKnobs;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    private int eqCount;

    void Start()
    {
        eqCount = eqKnobs.Length;
    }

    void Update()
    {
        for (int i = 0; i < eqCount; i++)
        {
            UpdateEQ(eqKnobs[i]);
        }
    }

    void UpdateEQ(EQKnob knob)
    {
        float rotationValue = Mathf.InverseLerp(knob.minRotation, knob.maxRotation, knob.knobTransform.localEulerAngles.z);
        float eqValue = Mathf.Lerp(-80f, 0f, rotationValue);  // Adjust the range as needed
        audioMixer.SetFloat(knob.parameterName, eqValue);
        Debug.Log($"Set EQ parameter {knob.parameterName} to {eqValue} dB");
    }
}

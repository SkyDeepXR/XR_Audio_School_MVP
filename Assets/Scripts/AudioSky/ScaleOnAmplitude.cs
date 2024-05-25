using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOnAmplitude : MonoBehaviour
{
    public float _startScale, _maxScale;
    public bool _useBuffer;
    private Material _material;
    public float _red, _green, _blue;

    // Start is called before the first frame update
    void Start()
    {
        _material = GetComponent<MeshRenderer>().materials[0];
    }

    // Update is called once per frame
    void Update()
    {
        float amplitude = AudioSpectralizer.amplitude;
        float amplitudeBuffer = AudioSpectralizer.amplitudeBuffer;

        if (!_useBuffer)
        {
            if (!float.IsNaN(amplitude))
            {
                float scaleValue = (amplitude * _maxScale) + _startScale;
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                
                // Uncomment and add NaN checks if needed
                // if (!float.IsNaN(_red) && !float.IsNaN(_green) && !float.IsNaN(_blue))
                // {
                //     Color _color = new Color(_red * amplitude, _green * amplitude, _blue * amplitude);
                //     _material.SetColor("_EmissionColor", _color);
                // }
            }
        }

        if (_useBuffer)
        {
            if (!float.IsNaN(amplitudeBuffer))
            {
                float scaleValue = (amplitudeBuffer * _maxScale) + _startScale;
                transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
                
                // Uncomment and add NaN checks if needed
                // if (!float.IsNaN(_red) && !float.IsNaN(_green) && !float.IsNaN(_blue))
                // {
                //     Color _color = new Color(_red * amplitudeBuffer, _green * amplitudeBuffer, _blue * amplitudeBuffer);
                //     _material.SetColor("_EmissionColor", _color);
                // }
            }
        }
    }
}
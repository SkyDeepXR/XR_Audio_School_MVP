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
        if (!_useBuffer)
        {
            transform.localScale = new Vector3((AudioSpectralizer.amplitude * _maxScale) + _startScale,
                (AudioSpectralizer.amplitude * _maxScale) + _startScale, (AudioSpectralizer.amplitude * _maxScale) + _startScale);
            // Color _color = new Color(_red * AudioSpectralizer.amplitude, _green * AudioSpectralizer.amplitude,
            //     _blue * AudioSpectralizer.amplitude);
            // _material.SetColor("_EmissionColor", _color);
        }

        if (_useBuffer)
        {
            transform.localScale = new Vector3((AudioSpectralizer.amplitudeBuffer * _maxScale) + _startScale,
                (AudioSpectralizer.amplitudeBuffer * _maxScale) + _startScale, (AudioSpectralizer.amplitudeBuffer * _maxScale) + _startScale);
            // Color _color = new Color(_red * AudioSpectralizer.amplitudeBuffer, _green * AudioSpectralizer.amplitudeBuffer,
            //     _blue * AudioSpectralizer.amplitudeBuffer);
            // _material.SetColor("_EmissionColor", _color);
        }
    }
}
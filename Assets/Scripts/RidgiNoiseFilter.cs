using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidgiNoiseFilter : INoiseFilter
{
    private Noise _noise = new Noise();
    private NoiseSetting.RidgiNoiseSetting _noiseSetting;

    public RidgiNoiseFilter(NoiseSetting.RidgiNoiseSetting _noiseSetting)
    {
        this._noiseSetting = _noiseSetting;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _noiseSetting.baseRoughness;

        //振幅
        float amplitude = 1;
        float weight = 1;
        for (int i = 0; i < _noiseSetting.numLayers; i++)
        {
            float v = 1 - Mathf.Abs(_noise.Evaluate(point * frequency + _noiseSetting.center));
            v *= v;
            v *= weight;

            weight = Mathf.Clamp01(v * _noiseSetting.weight);

            noiseValue += v * amplitude;

            frequency *= _noiseSetting.roughness;
            amplitude *= _noiseSetting.persistence;
        }

        //noiseValue = Mathf.Max(0, noiseValue - _noiseSetting.minvalue);
        noiseValue = noiseValue - _noiseSetting.minvalue;
        
        return noiseValue * _noiseSetting.strength;
    }
}
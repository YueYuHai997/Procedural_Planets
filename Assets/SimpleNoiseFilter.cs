using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNoiseFilter : INoiseFilter
{
    private Noise _noise = new Noise();
    private NoiseSetting.SimpleNoiseSetting _noiseSetting;

    public SimpleNoiseFilter(NoiseSetting.SimpleNoiseSetting _noiseSetting)
    {
        this._noiseSetting = _noiseSetting;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _noiseSetting.baseRoughness;

        //振幅
        float amplitude = 1;

        for (int i = 0; i < _noiseSetting.numLayers; i++)
        {
            float v = _noise.Evaluate(point * frequency + _noiseSetting.center);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= _noiseSetting.roughness;
            amplitude *= _noiseSetting.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _noiseSetting.minvalue);

        return noiseValue * _noiseSetting.strength;
    }
}
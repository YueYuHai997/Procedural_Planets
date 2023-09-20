using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    private ShapeSetting _shapeSetting;
    
    private INoiseFilter[] _noiseFilters;
    
    
    public ShapeGenerator(ShapeSetting _shapeSetting)
    {
        this._shapeSetting = _shapeSetting;
        _noiseFilters = new INoiseFilter[_shapeSetting.noiseLayer.Length];
        for (int i = 0; i < _shapeSetting.noiseLayer.Length; i++)
        {
            _noiseFilters[i] = NoiseFilterFactor.CreateNoiseFilter(_shapeSetting.noiseLayer[i].NoiseSetting);
        }
    }

    public Vector3 CaculatePointOnPlane(Vector3 pointOnUnitSphere)
    {
        float firstLayerValue = 0;
        float elevation = 0;

        if (_noiseFilters.Length > 0)
        {
            firstLayerValue = _noiseFilters[0].Evaluate(pointOnUnitSphere);
            if (_shapeSetting.noiseLayer[0].enable)
            {
                elevation = firstLayerValue;
            }
        }
        //节约资源循环该为 从0开始 
        for (int i = 1; i < _noiseFilters.Length; i++)
        {
            if (_shapeSetting.noiseLayer[i].enable)
            {
                float mask = _shapeSetting.noiseLayer[i].firstLayerMask ? elevation : 1;
                elevation += _noiseFilters[i].Evaluate(pointOnUnitSphere) * mask;
            }
        }
        
        return pointOnUnitSphere * ((1 + elevation) * _shapeSetting.plantRadius); 
    }
}

using System;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu()]
public class ColorSetting : ScriptableObject
{ 
    public Material planetmaterial;
    
    //每个部落群都可以有各自的颜色设置
    public BiomeColorSetting biomeColorSetting;

    public Gradient oceanColor;
    
    [System.Serializable]
    public class BiomeColorSetting
    {
        public Biome[] biomes;

        public NoiseSetting noiseSetting;
        
        public float noiseOffset;
        
        public float noiseStrength;
        //融合程度
        [Range(0, 1)] public float blendAmount;

        [System.Serializable]
        public class Biome
        {
            public Gradient gradient;

            /// <summary> 底色 </summary>
            public Color tint;

            [Range(0, 1)] public float startHeight;

            /// <summary> 底色百分比 </summary>
            [Range(0, 1)] public float tinePercent;
        }
    }
}
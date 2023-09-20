using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class ShapeSetting : ScriptableObject
{
    public float plantRadius = 1; 
    public NoiseLayer[] noiseLayer;
    
    
    [System.Serializable]
    public class  NoiseLayer
    {
        public bool enable; //控制是否启用
        public bool firstLayerMask;  // 是否将第一层作为Mask  如果关闭的话 可以视为叠加模式，如果开启会在第一层的基础上进行改变
        public NoiseSetting NoiseSetting;
    }
}
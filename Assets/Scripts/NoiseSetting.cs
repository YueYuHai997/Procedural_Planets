using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class NoiseSetting
{
    public enum FilterType
    {
        Simple,
        Ridgid,
    }

    public FilterType filterType;

    [ConditionalHideAttribute("filterType",0)]
    public SimpleNoiseSetting simpleNoiseSetting;
    
    [ConditionalHideAttribute("filterType",1)]
    public RidgiNoiseSetting ridgiNoiseSetting;
    
    
    [System.Serializable]
    public class SimpleNoiseSetting
    {
        //强度
        public float strength = 1;

        //总计层数 越低的层 平滑越低 振幅越小 表现越不明显 
        [Range(1, 8)] public int numLayers = 1;

        //基础平平滑度
        public float baseRoughness = 1;

        //平滑度衰减
        public float roughness = 2;

        //公共浮动振幅衰减
        public float persistence = .5f;

        //噪声中心
        public Vector3 center = Vector3.zero;

        //最小值
        public float minvalue;
    }

    
    [System.Serializable]
    public class RidgiNoiseSetting : SimpleNoiseSetting
    {
        public float weight;
    }
}
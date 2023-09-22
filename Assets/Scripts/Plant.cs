using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Plant : MonoBehaviour
{
    [Range(2, 256)] public int reslution = 10;

    [SerializeField, HideInInspector] private MeshFilter[] _meshFilters;

    private TerrainFace[] _terrainFaces;

    public enum FaceRenderMask { All, Top, Down, Left, Right, Front, Back }

    public FaceRenderMask faceRenderMask;

    public ShapeSetting shapeSetting;
    public ColorSetting colorSetting;

    private ShapeGenerator _shapeGenerator = new ShapeGenerator();
    private ColorGenerator _colorGenerator = new ColorGenerator();
    

    void Initialize()
    {
        _shapeGenerator.UpdateSettings(shapeSetting);
        _colorGenerator.UpdateSettings(colorSetting);

        if (_meshFilters == null || _meshFilters.Length == 0)
        {
            _meshFilters = new MeshFilter[6];
        }

        _terrainFaces = new TerrainFace[6];
        
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i] == null)
            {
                GameObject meshobj = new GameObject("mesh");
                meshobj.transform.parent = this.transform;

                meshobj.AddComponent<MeshRenderer>();

                _meshFilters[i] = meshobj.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }

            _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = colorSetting.planetmaterial;
            _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, reslution, directions[i]);
            bool renderFace = faceRenderMask == FaceRenderMask.All || (int)faceRenderMask - 1 == i;
            _meshFilters[i].gameObject.SetActive(renderFace);
        }
    }

    void GenerateMesh()
    {
        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i].gameObject.activeSelf)
            {
                _terrainFaces[i].ConstrictMesh();
            }
        }
        _colorGenerator.UpdateElevation(_shapeGenerator.minMax);
    }

    void GenerateColor()
    {
        _colorGenerator.UpdateColors();
        
        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i].gameObject.activeSelf)
            {
                _terrainFaces[i].UpdateUVs(_colorGenerator);
            }
        }
    }

    private void Update()
    {
        GeneratePlanet();
    }


    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColor();
    }

    public void OnShapeSettingUpdate()
    {
        Initialize();
        GenerateMesh();
    }

    public void OnColorSettingsUpdate()
    {
        Initialize();
        GenerateColor();
    }

    private void OnValidate()
    {
        GeneratePlanet();
    }
}
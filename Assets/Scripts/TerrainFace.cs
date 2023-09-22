using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private Mesh _mesh;

    private int _resolution;

    private Vector3 _localUp;

    private Vector3 _axisA;

    private Vector3 _asisB;

    private ShapeGenerator _shapeGenerator;

    public TerrainFace(ShapeGenerator _shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        this._mesh = mesh;
        this._resolution = resolution;
        this._localUp = localUp;
        this._shapeGenerator = _shapeGenerator;

        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _asisB = -Vector3.Cross(_localUp, -_axisA);
    }

    public void ConstrictMesh()
    {
        //存储顶点 ，使用_resolution 描述mesh的细分程度
        Vector3[] vertices = new Vector3[_resolution * _resolution];

        //三角形顶点序列 由推测可得 使用_resolution个顶点可形成 _resolution-1 *  _resolution-1 个面
        // 一个面是有两个三角形组成的 一个三角形拥有三个顶点 所以共有  _resolution-1 *  _resolution-1 *2 * 3 个顶点
        int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];

        int triIndex = 0;
        //保存UV信息
        Vector2[] uv = (_mesh.uv.Length == vertices.Length) ? _mesh.uv : new Vector2[vertices.Length];

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = y * _resolution + x;

                //计算每个顶点的百分比 规划为（0,0）到（1,1）
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                //计算每个顶点的坐标 通过上面计算出的顶点百分比
                //_localUp 默认为 当前平面到（0,0,0）点的位移 ，_axisA，_asisB 分别代表两个轴
                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _asisB;
                //将正方形 形状改为 圆形
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                float unscaleElevation = _shapeGenerator.CalculataUnscaleElevation(pointOnUnitSphere);
                vertices[i] = pointOnUnitSphere * _shapeGenerator.GetScaleElevation(unscaleElevation);

                uv[i].y = unscaleElevation;
                
                //构建三角形顶点序列 通过顺时针的方式 同时排除最右侧和最底部的顶点
                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    triangles[triIndex++] = i;
                    triangles[triIndex++] = i + _resolution + 1;
                    triangles[triIndex++] = i + _resolution;

                    triangles[triIndex++] = i;
                    triangles[triIndex++] = i + 1;
                    triangles[triIndex++] = i + _resolution + 1;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        //重新计算法线
        _mesh.RecalculateNormals();
        _mesh.uv = uv;
    }


    public void UpdateUVs(ColorGenerator colorGenerator)
    {
        Vector2[] uv = _mesh.uv;
        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = y * _resolution + x;

                //计算每个顶点的百分比 规划为（0,0）到（1,1）
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                //计算每个顶点的坐标 通过上面计算出的顶点百分比
                //_localUp 默认为 当前平面到（0,0,0）点的位移 ，_axisA，_asisB 分别代表两个轴
                Vector3 pointOnUnitCube = _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _asisB;
                //将正方形 形状改为 圆形
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

                //重新构建UV 一个生物群落使用同一个uv
                uv[i].x = colorGenerator.BiomePercentFromPoint(pointOnUnitSphere);
            }
        }

        _mesh.uv = uv;
    }
}
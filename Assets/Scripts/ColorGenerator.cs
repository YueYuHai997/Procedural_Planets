using UnityEngine;

public class ColorGenerator
{
    private ColorSetting _colorSetting;
    private Texture2D _texture2D;
    private readonly int _textureResolution = 50;
    private INoiseFilter _noiseFilter;

    private static readonly int Texture1 = Shader.PropertyToID("_texture");
    private static readonly int ElevationMinMax = Shader.PropertyToID("_elevationMinMax");

    public void UpdateSettings(ColorSetting colorSetting)
    {
        this._colorSetting = colorSetting;
        if (_texture2D == null || _texture2D.height != _colorSetting.biomeColorSetting.biomes.Length)
            //使用前半部分处理海洋颜色 ，禁用MiniMap
            _texture2D = new Texture2D(_textureResolution * 2, _colorSetting.biomeColorSetting.biomes.Length,
                TextureFormat.RGBA32, false);
        _noiseFilter = NoiseFilterFactor.CreateNoiseFilter(_colorSetting.biomeColorSetting.noiseSetting);
    }

    public void UpdateElevation(MinMax elevationMinMax)
    {
        _colorSetting.planetmaterial.SetVector(ElevationMinMax,
            new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
    {
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        heightPercent += (_noiseFilter.Evaluate(pointOnUnitSphere) - _colorSetting.biomeColorSetting.noiseOffset) *
                         _colorSetting.biomeColorSetting.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = _colorSetting.biomeColorSetting.biomes.Length;
        float blendRande = _colorSetting.biomeColorSetting.blendAmount / 2f + .001f;
        for (int i = 0; i < numBiomes; i++)
        {
            float dst = heightPercent - _colorSetting.biomeColorSetting.biomes[i].startHeight;
            float weight = Mathf.InverseLerp(blendRande, -blendRande, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, numBiomes - 1);
    }

    public void UpdateColors()
    {
        Color[] colors = new Color[_texture2D.width * _texture2D.height];
        int colorIndex = 0;
        foreach (var biome in _colorSetting.biomeColorSetting.biomes)
        {
            for (int i = 0; i < _textureResolution * 2; i++)
            {
                Color gradientCol;
                if (i < _textureResolution)
                {
                    gradientCol = _colorSetting.oceanColor.Evaluate(i / (_textureResolution - 1f));
                }
                else
                {
                    gradientCol = biome.gradient.Evaluate((i - _textureResolution) / (_textureResolution - 1f));
                }
                Color tintCol = biome.tint;
                // 进行颜色融合
                colors[colorIndex] = gradientCol * (1 - biome.tinePercent) + tintCol * biome.tinePercent;
                colorIndex++;
            }
        }

        _texture2D.SetPixels(colors);
        _texture2D.Apply();
        _colorSetting.planetmaterial.SetTexture(Texture1, _texture2D);
    }
}
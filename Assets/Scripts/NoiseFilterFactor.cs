using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseFilterFactor
{
    public static INoiseFilter CreateNoiseFilter(NoiseSetting noiseSetting)
    {
        switch (noiseSetting.filterType)
        {
            case NoiseSetting.FilterType.Ridgid:
                return new RidgiNoiseFilter(noiseSetting.ridgiNoiseSetting);
            case NoiseSetting.FilterType.Simple:
                return new SimpleNoiseFilter(noiseSetting.simpleNoiseSetting);
        }

        return null;
    }
}
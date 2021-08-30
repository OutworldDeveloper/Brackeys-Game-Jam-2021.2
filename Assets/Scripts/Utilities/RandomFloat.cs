using System;

[Serializable]
public struct RandomFloat
{
    public float MinValue;
    public float MaxValue;

    public float Generate()
    {
        return UnityEngine.Random.Range(MinValue, MaxValue);
    }

}

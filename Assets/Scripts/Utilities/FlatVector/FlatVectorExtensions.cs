using UnityEngine;

public static class FlatVectorExtensions
{

    public static FlatVector GetFlatPosition(this Transform transform)
    {
        return new FlatVector(transform.position);
    }

}
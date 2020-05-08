using UnityEngine;

public static class NumericExtensions
{
    public static float Remap(this float source, float fromMin, float fromMax, float toMin, float toMax)
    {
        source = Mathf.Clamp(source, fromMin, fromMax);

        var fromAbs = source - fromMin;
        var fromMaxAbs = fromMax - fromMin;

        var normal = fromAbs / fromMaxAbs;

        var toMaxAbs = toMax - toMin;
        var toAbs = toMaxAbs * normal;

        var to = toAbs + toMin;

        return to;
    }
}